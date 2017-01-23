using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject Room1;
    public GameObject Room2;
    public GameObject Room3;

    private int currentRoom;

    private gameManager manager;

    private bool moveSize = false;
    private float targetSize;

    private bool movePosition = false;
    private Vector3 targetPosition;

    void Start()
    {
        Room2.SetActive(false);
        Room3.SetActive(false);

        manager = GameObject.Find("gameManager").GetComponent<gameManager>();

        currentRoom = 1;

        Room1.GetComponentInChildren<WaveCreator>().DrawLines();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            OpenRoom(2);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            OpenRoom(2);
            OpenRoom(3);
        }

        if (moveSize)
        {
            if (Camera.main.orthographicSize <= targetSize)
            {
                Camera.main.orthographicSize += (Time.deltaTime);
            }
            else
            {
                moveSize = false;
            }
        }

        if (movePosition)
        {

            if (Vector3.Distance(targetPosition, Camera.main.transform.position) > 1f)
            {
                float xDifference = (targetPosition.x - Camera.main.transform.position.x) * Time.deltaTime;
                float yDifference = (targetPosition.y - Camera.main.transform.position.y) * Time.deltaTime;
                Camera.main.transform.position += new Vector3(xDifference, yDifference, 0); //.Translate(new Vector3(xDifference, yDifference, 0));
            }
            else
            {
                movePosition = false;
            }
        }
    }

    public void OpenRoom(int roomIndex)
    {
        if (roomIndex == 2 && currentRoom < 2)
        {
            Room2.SetActive(true);

            targetSize = 8f;
            moveSize = true;

            targetPosition = new Vector3(7.5f, -3.1f, -10);
            movePosition = true;

            currentRoom = 2;

            WaveCreator[] creators = Room2.GetComponentsInChildren<WaveCreator>();
            foreach (WaveCreator creator in creators)
                creator.DrawLines();
        }
        else if (roomIndex == 3 && currentRoom < 3)
        {
            Room3.SetActive(true);

            targetSize = 10f;
            moveSize = true;

            targetPosition = new Vector3(10.3f, -3.3f, -10);
            movePosition = true;

            currentRoom = 3;

            WaveCreator[] creators = Room3.GetComponentsInChildren<WaveCreator>();
            foreach (WaveCreator creator in creators)
                creator.DrawLines();
        }
        else if (roomIndex == 4)
        {
            manager.EndGame(true);
        }
    }
}
