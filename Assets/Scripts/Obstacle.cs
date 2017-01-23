using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleTypeEnum ObstacleType;

    public GameObject RoomSourceObject;

    void OnMouseDown()
    {
        GameObject.Find("gameManager").GetComponent<gameManager>().ShowSelectionsForSelectedObstacle(GetComponent<Obstacle>());
    }

    public void RemoveObstacle()
    {
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        RoomSourceObject.GetComponent<WaveCreator>().DrawLines();
    }
}


public enum ObstacleTypeEnum
{
    Cable = 1,
    Chip = 2,
    Receiver = 3
}