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

        //waveCreator = RoomSourceObject.GetComponent<WaveCreator>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        List<RaycastHit> hits = new List<RaycastHit>();

        RaycastHit hit;

        List<bool> hasLightList = new List<bool>();

        for (int i = 0; i < RoomSourceObject.Length; i++)
        {
            Transform currentPosition = RoomSourceObject[i].transform;
            WaveCreator currentWaveCreator = waveCreatorList[i];

            Physics.Linecast(currentPosition.position, this.transform.position, out hit, layerMask);

            if (hit.collider != null && ((hit.collider.gameObject.tag == "Obstacle"
                && !currentWaveCreator.DeletedObstacleNames.Any(obs => obs == hit.collider.gameObject.name || obs == hit.collider.transform.parent.gameObject.name))
                || hit.collider.gameObject.tag == "Mirror"))
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Props/kapi 1");
                hasLightList.Add(false);
            }
            else
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Props/kapi2");
                hasLightList.Add(true);
            }
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

            if (hit.collider != null && ((hit.collider.gameObject.tag == "Obstacle"
                && !currentWaveCreator.DeletedObstacleNames.Any(obs => obs == hit.collider.gameObject.name || obs == hit.collider.transform.parent.gameObject.name))
                || hit.collider.gameObject.tag == "Mirror"))
            {
                hasLightList.Add(false);
            }
            else
            {
                hasLightList.Add(true);
            }
        }

        if (hasLightList.Any(light => light != true))
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Props/kapi 1");
            hasLight = false;
        }
        else
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Props/kapi2");
            hasLight = true;
        }
    }

    public bool IsInCorrectPosition()
    {
        if (ShouldHaveLight == hasLight)
            return true;
        else
            return false;
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
}
