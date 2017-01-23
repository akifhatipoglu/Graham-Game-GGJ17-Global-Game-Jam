using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class gameManager : MonoBehaviour
{
    public int Lives = 3;
    public int Attempts = 0;

    public Text message;
    public AudioClip badEndSong;

    public Obstacle selectedObstacle;

    private GameObject selectionPanel;
    private GameObject endGameCanvas;
    private ShowPanels showPanels;
    private AudioSource audioSource;

    private bool gameEnded;

    private List<string> grahamsThoughts = new List<string>()
    {
        ("I need to clear the disturbances in the line to reach Mr. Watson..."),
        ("I miss Mr. Watson... I wonder if something happened to him..."),
        ("Mr. Watson we are calling from maintenance office,  can you please blow to your reciever!"),
        ("I need to clear the disturbances in the line to reach Mr. Watson..."),
        ("My dear Mr. Watson..."),
        ("These schemes on the wall are wrong alltogether."),
        ("I need to clear the disturbances in the line to reach Mr. Watson..."),
        ("Watson since I've been loving you..."),
        ("I believe I should use both sources on the last room.")
    };
    private Text grahamsThoughtText;
    private float changeThoughtTimer = 13.0f;
    private int thoughtIndex = 0;
    private bool successfullyEnded = false;
    void Awake()
    {
        if (GameObject.Find("UI") != null)
            showPanels = GameObject.Find("UI").GetComponent<ShowPanels>();

        audioSource = GameObject.Find("gameManager").GetComponent<AudioSource>();
    }

    void Start()
    {
        gameEnded = false;

        endGameCanvas = GameObject.Find("EndGameCanvas");
        endGameCanvas.SetActive(false);

        selectionPanel = GameObject.Find("SelectionPanel");
        FeedSelectionPanel(selectionPanel);
        selectionPanel.SetActive(false);

        grahamsThoughtText = GameObject.Find("Graham's Mood Text").GetComponent<Text>();

        Lives = 5;
        Attempts = 0;
        FeedLivesText();
        UpdateAttempts();
    }

    void Update()
    {
        changeThoughtTimer -= Time.deltaTime;

        if (changeThoughtTimer <= 0 && !gameEnded)
        {
            grahamsThoughtText.text = grahamsThoughts[thoughtIndex % grahamsThoughts.Count];
            thoughtIndex++;
            changeThoughtTimer = 13.0f;

            audioSource.clip = Resources.Load<AudioClip>("Audio/grahamBozukSes");
            audioSource.Play();
        }

        if (Input.GetKeyUp(KeyCode.E))
            EndGame(true);
    }

    private void FeedSelectionPanel(GameObject selectionPanel)
    {
        GameObject firstSelection = Instantiate(Resources.Load("Prefabs/SelectionItem_Cable") as GameObject);
        GameObject secondSelection = Instantiate(Resources.Load("Prefabs/SelectionItem_Chip") as GameObject);
        GameObject thirdSelection = Instantiate(Resources.Load("Prefabs/SelectionItem_Receiver") as GameObject);

        firstSelection.transform.SetParent(selectionPanel.transform);
        firstSelection.transform.localScale = new Vector3(1, 2, 1);
        firstSelection.transform.localPosition = new Vector3(0, 10, 0);
        firstSelection.GetComponent<SelectionItem>().ObstacleType = ObstacleTypeEnum.Cable;
        firstSelection.GetComponent<SelectionItem>().SendMessage("Initialize");

        secondSelection.transform.SetParent(selectionPanel.transform);
        secondSelection.transform.localScale = new Vector3(1, 2, 1);
        secondSelection.transform.localPosition = new Vector3(0, -20, 0);
        secondSelection.GetComponent<SelectionItem>().ObstacleType = ObstacleTypeEnum.Chip;
        secondSelection.GetComponent<SelectionItem>().SendMessage("Initialize");

        thirdSelection.transform.SetParent(selectionPanel.transform);
        thirdSelection.transform.localScale = new Vector3(1, 2, 1);
        thirdSelection.transform.localPosition = new Vector3(0, -50, 0);
        thirdSelection.GetComponent<SelectionItem>().ObstacleType = ObstacleTypeEnum.Receiver;
        thirdSelection.GetComponent<SelectionItem>().SendMessage("Initialize");
    }

    public void FeedLivesText()
    {
        GameObject.Find("Lives Value").GetComponent<Text>().text = Lives.ToString();
    }

    public void UpdateAttempts()
    {
        GameObject.Find("Attempts Value").GetComponent<Text>().text = Attempts.ToString();
    }

    public void IncreaseAttemptCount()
    {
        Attempts++;
        UpdateAttempts();
    }

    public void DecreaseLives()
    {
        this.Lives--;
        FeedLivesText();

        if (this.Lives == 0)
            EndGame(false);
    }

    public void ShowSelectionsForSelectedObstacle(Obstacle obstacle)
    {
        selectionPanel.SetActive(true);
        selectedObstacle = obstacle;
    }

    public void EndGame(bool successful)
    {

        if (successful)
            successfullyEnded = true;
        else
            successfullyEnded = false;

        endGameCanvas.SetActive(true);
        StartCoroutine(CloseScreen());
    }

    private IEnumerator CloseScreen()
    {
        endGameCanvas.GetComponentInChildren<Image>().CrossFadeAlpha(255f, 2.0f, false);
        yield return new WaitForSeconds(2f);
        OpenMainMenu();
    }

    public void OpenMainMenu()
    {
        gameEnded = true;
        if (showPanels != null)
        {
            showPanels.ShowMenu();
            if (successfullyEnded)
            {
                GameObject[] hasAudioObjects = GameObject.FindGameObjectsWithTag("HasAudio");

                for (int i = 0; i < hasAudioObjects.Length; i++)
                {
                    hasAudioObjects[i].GetComponent<AudioSource>().Stop();
                }

                audioSource.clip = Resources.Load<AudioClip>("Audio/grahamSes");
                audioSource.Play();

                showPanels.SetEndingMessage(true);
            }
            else
                showPanels.SetEndingMessage(false);
        }
    }
}
