using UnityEngine;
using UnityEngine.UI;

public class SelectionItem : MonoBehaviour
{
    public AudioClip successSelectionClip;
    public AudioClip failedSelectionClip;

    public ObstacleTypeEnum ObstacleType { get; set; }

    private GameObject selectionPanel;
    private gameManager manager;

    private AudioSource audioSource;

    void Start()
    {
        selectionPanel = GameObject.Find("SelectionPanel");
        manager = GameObject.Find("gameManager").GetComponent<gameManager>();
        audioSource = GameObject.Find("gameManager").GetComponent<AudioSource>();
    }

    void Initialize()
    {
        //this.GetComponentInChildren<Text>().text = this.ObstacleType.ToString();
    }

    public void SelectionMade()
    {
        if (this.ObstacleType == manager.selectedObstacle.ObstacleType)
        {
            audioSource.clip = successSelectionClip;
            audioSource.Play();
            SetSuccessfulSelection();
        }
        else
        {
            audioSource.clip = failedSelectionClip;
            audioSource.Play();
            SetFailedSelection();
        }
    }

    private void SetSuccessfulSelection()
    {
        manager.IncreaseAttemptCount();

        GameObject sourceObject = manager.selectedObstacle.RoomSourceObject;
        WaveCreator currentWaveCreator = sourceObject.GetComponent<WaveCreator>();

        GameObject clickedObstacle = manager.selectedObstacle.gameObject;
        if (clickedObstacle.GetComponent<MeshRenderer>() != null)
            clickedObstacle.GetComponent<MeshRenderer>().enabled = true;

        if (clickedObstacle.GetComponent<Collider>() != null)
            clickedObstacle.GetComponent<Collider>().enabled = false;

        foreach (Transform child in clickedObstacle.transform)
        {
            MeshRenderer mesh = child.gameObject.GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                mesh.enabled = true;
            }

            if (child.GetComponent<Collider>() != null)
                child.GetComponent<Collider>().enabled = false;
        }

        selectionPanel.SetActive(false);
        currentWaveCreator.DrawLines();
    }

    private void SetFailedSelection()
    {
        manager.IncreaseAttemptCount();
        manager.DecreaseLives();
    }
}
