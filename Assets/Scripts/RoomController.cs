using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public GameObject Doors;
    public GameObject Mirrors;
    public GameObject Sources;
    public int NextRoomIndex;

    private List<Door> doors = new List<Door>();
    private List<Mirror> mirrors = new List<Mirror>();

    void Start()
    {
        foreach (Transform child in Doors.transform)
            doors.Add(child.GetComponent<Door>());

        foreach (Transform child in Mirrors.transform)
            mirrors.Add(child.GetComponent<Mirror>());
    }

    public void CheckDoors()
    {
        if (doors.Count == 0)
            return;

        foreach (Door door in doors)
        {
            door.SetState();
        }

        foreach (Door door in doors)
        {
            if (!door.IsInCorrectPosition())
                return;
        }

        GameObject.Find("Controller").GetComponent<GameController>().OpenRoom(NextRoomIndex);
    }
}
