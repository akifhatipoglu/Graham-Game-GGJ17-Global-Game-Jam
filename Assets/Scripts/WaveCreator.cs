using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveCreator : MonoBehaviour
{
    public GameObject RoomController;
    public int RoomWidthLength;
    public int RoomHeightLength;

    public List<string> DeletedObstacleNames = new List<string>();
    public List<Vector3> MirrorLines = new List<Vector3>();

    private List<GameObject> Lines = new List<GameObject>();
    private RoomController roomController;

    private LayerMask layerMask;

    void Start()
    {
        layerMask = ~(1 << LayerMask.NameToLayer("Source") | 1 << LayerMask.NameToLayer("Door"));

        roomController = RoomController.GetComponent<RoomController>();
    }

    public void DrawLines()
    {
        foreach (GameObject line in Lines)
            Destroy(line);
        Lines.Clear();
        MirrorLines.Clear();

        for (int angle = 270; angle < 360; angle++)
        {
            Vector3 targetVector = new Vector3(this.transform.position.x + ((float)Mathf.Cos(Mathf.Deg2Rad * angle) * 50),
                                                this.transform.position.y + ((float)Mathf.Sin(Mathf.Deg2Rad * angle) * 50),
                                                this.transform.position.z);

            Debug.Log(targetVector);
            DrawLine(this.transform.position, targetVector);
        }

        roomController.CheckDoors();
    }

    private void DrawLinesToRight(int index)
    {
        Vector3 targetPos = new Vector3(this.transform.position.x + 9.5f, this.transform.position.y - ((float)index / 2), 0);
        DrawLine(this.transform.position, targetPos);
    }

    private void DrawLinesToBottom(int index)
    {
        Vector3 targetPos = new Vector3(this.transform.position.x + ((float)index / 2), this.transform.position.y - 6f, 0);
        DrawLine(this.transform.position, targetPos);
    }

    private void DrawLine(Vector3 source, Vector3 target)
    {
        GameObject linePrefab = Instantiate(Resources.Load("Prefabs/LineWithElectricity") as GameObject);
        linePrefab.transform.SetParent(this.transform);

        Transform lineTransform = linePrefab.GetComponentInChildren<Line>().transform;
        lineTransform.position = source;

        Lines.Add(linePrefab);

        LineRenderer line = linePrefab.GetComponent<LineRenderer>();
        line.SetPosition(0, source);

        RaycastHit hit;
        Physics.Linecast(source, target, out hit, layerMask);

        if (hit.collider != null && hit.collider.gameObject != null && ((hit.collider.gameObject.tag == "Obstacle" &&
            !DeletedObstacleNames.Any(obs => obs == hit.collider.gameObject.name || obs == hit.collider.transform.parent.gameObject.name)) ||
            hit.collider.gameObject.tag == "Wall"))
        {
            line.SetPosition(1, hit.point);
            linePrefab.GetComponentInChildren<Line>().MoveTowards(hit.point);
        }
        else if (hit.collider != null && hit.collider.gameObject.tag == "Mirror")
        {
            line.SetPosition(1, hit.point);
            //linePrefab.GetComponentInChildren<Line>().MoveTowards(hit.point);

            DrawNewMirrorLine(lineTransform, hit);
        }
        else
        {
            line.SetPosition(1, target);
            //linePrefab.GetComponentInChildren<Line>().MoveTowards(target);
        }
    }

    private void DrawNewMirrorLine(Transform sourceLine, RaycastHit hit)
    {
        Vector3 reflected = Vector3.Reflect(sourceLine.position, getReflectionVector(hit.collider.gameObject.transform.rotation.z));

        Debug.DrawRay(hit.point, hit.normal, Color.magenta, 10f);

        GameObject linePrefab = Instantiate(Resources.Load("Prefabs/LineWithElectricity") as GameObject);
        linePrefab.transform.SetParent(this.transform);
        linePrefab.gameObject.name += "_mirror";
        Transform lineTransform = linePrefab.GetComponentInChildren<Line>().transform;

        Lines.Add(linePrefab);

        LineRenderer line = linePrefab.GetComponent<LineRenderer>();
        line.SetPosition(0, hit.point);

        RaycastHit newHit;
        Physics.Linecast(hit.point, reflected * 100, out newHit, layerMask);

        if (newHit.collider != null)
        {
            line.SetPosition(1, newHit.point);
            //linePrefab.GetComponentInChildren<Line>().MoveTowards(newHit.point);
            MirrorLines.Add(newHit.point);
        }
        else
        {
            line.SetPosition(1, hit.point);
        }
    }

    private Vector3 getReflectionVector(float rotation)
    {

        Vector3 result = Vector3.zero;
        if ((rotation > -0.75f && rotation < -0.65f) || (rotation > 0.68f && rotation < 0.72f))
        {
            result = Vector3.right;
        }
        else if ((rotation > -0.01f && rotation < 0.01f) || (rotation > 0.99f && rotation < 1.01f) ||
            (rotation < -0.99f && rotation > -1.01f))
        {
            result = Vector3.up;
        }
        //else if((rotation < -0.91 && rotation > -0.93) || (rotation > 0.91 && rotation < 0.93))
        //    //(rotation > 0.37 && rotation < 0.39))
        //    //(rotation < -0.37 && rotation > -0.39) || (rotation > 0.37 && rotation < 0.39))
        //{
        //    result = new Vector3(1f, -1f, 0);
        //}
        //else
        //{
        //    result = new Vector3(-1f, 1f, 0);
        //}

        Debug.Log("rotation: " + rotation + " result: " + result);
        return result;
    }
}
