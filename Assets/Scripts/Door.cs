using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject[] RoomSourceObject;
    public bool ShouldHaveLight;

    private List<WaveCreator> waveCreatorList = new List<WaveCreator>();
    private SpriteRenderer spriteRenderer;
    private bool hasLight;

    LayerMask layerMask;
    void Start()
    {
        layerMask = ~(1 << LayerMask.NameToLayer("Source") | 1 << LayerMask.NameToLayer("Door"));

        CheckWaveCreatorList();

        spriteRenderer = this.GetComponent<SpriteRenderer>();

        List<RaycastHit> hits = new List<RaycastHit>();

        RaycastHit hit;

        List<bool> hasLightList = new List<bool>();

        for (int i = 0; i < RoomSourceObject.Length; i++)
        {
            Transform currentPosition = RoomSourceObject[i].transform;
            WaveCreator currentWaveCreator = waveCreatorList[i];

            Physics.Linecast(currentPosition.position, this.transform.position, out hit, layerMask);

            if (AnyRayHitsTheDoor(hit, currentWaveCreator))
                Open();
            else
                Close();
        }

        if (hasLightList.Any(light => light == true))
            hasLight = true;
        else
            hasLight = false;
    }

    public void SetState()
    {
        List<bool> hasLightList = new List<bool>();

        CheckWaveCreatorList();
        for (int i = 0; i < RoomSourceObject.Length; i++)
        {
            Transform currentPosition = RoomSourceObject[i].transform;
            WaveCreator currentWaveCreator = waveCreatorList[i];

            RaycastHit hit;
            Physics.Linecast(currentPosition.position, this.transform.position, out hit, layerMask);

            if (AnyRayHitsTheDoor(hit, currentWaveCreator))
                hasLightList.Add(true);
            else
                hasLightList.Add(false);
        }

        if (hasLightList.Any(light => light != true))
            Close();
        else
            Open();
    }

    public bool IsInCorrectPosition()
    {
        return ShouldHaveLight == hasLight;
    }

    private void CheckWaveCreatorList()
    {
        if (waveCreatorList != null && waveCreatorList.Count > 0)
            return;

        waveCreatorList = new List<WaveCreator>();
        foreach (GameObject sourceObject in RoomSourceObject)
        {
            waveCreatorList.Add(sourceObject.GetComponent<WaveCreator>());
        }
    }

    private bool AnyRayHitsTheDoor(RaycastHit hit, WaveCreator currentWaveCreator)
    {
        bool hasObstacle = hit.collider != null && ((hit.collider.gameObject.tag == "Obstacle"
            && !currentWaveCreator.DeletedObstacleNames.Any(obs => obs == hit.collider.gameObject.name || obs == hit.collider.transform.parent.gameObject.name))
            || hit.collider.gameObject.tag == "Mirror");

        if (hasObstacle)
        {
            foreach (var hitMirrorLine in currentWaveCreator.MirrorLines.Where(line => Vector3.Distance(line, this.transform.position) < 1.0f))
            {
                RaycastHit mirrorLineHit;
                Physics.Linecast(hitMirrorLine, this.transform.position, out mirrorLineHit, layerMask);

                if (mirrorLineHit.collider != null && ((mirrorLineHit.collider.gameObject.tag == "Obstacle" &&
                    !currentWaveCreator.DeletedObstacleNames.Any(obs => obs == hit.collider.gameObject.name || obs == hit.collider.transform.parent.gameObject.name))
                    || mirrorLineHit.collider.gameObject.tag == "Mirror"))
                {
                    continue;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        return true;
    }

    private void Open()
    {
        spriteRenderer.sprite = Resources.Load<Sprite>("Props/kapi2");
        hasLight = true;
    }

    private void Close()
    {
        spriteRenderer.sprite = Resources.Load<Sprite>("Props/kapi 1");
        hasLight = false;
    }
}
