using UnityEngine;

public class Mirror : MonoBehaviour
{
    public GameObject RoomSourceObject;

    private gameManager manager;

    void Start()
    {
        manager = GameObject.Find("gameManager").GetComponent<gameManager>();
    }

    void OnMouseDown()
    {
        RotateMirror();
    }

    void RotateMirror()
    {
        this.transform.Rotate(new Vector3(0, 0, 1), -90);
        manager.IncreaseAttemptCount();
        RoomSourceObject.GetComponent<WaveCreator>().DrawLines();
    }
}
