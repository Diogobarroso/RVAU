using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Vuforia;

public class GameManager : MonoBehaviour
{
    List<GameObject> powerUps;
    List<GameObject> activePowerUps;
    float probability = 0.02f; //Probability [0,1] with which a power up will spawn
    bool over = false;
    GameObject[] players;
    bool gameActive = false;

    private bool waitingToStart = true;

    [SerializeField]
    private GameObject p1;

    [SerializeField]
    private GameObject p2;

    [SerializeField]
    private GameObject title;

    [SerializeField]
    private GameObject instructions;

    private PlayerBehaviour p1Script;

    private PlayerBehaviour p2Script;



    void Start()
    {

        p1Script = p1.GetComponent<PlayerBehaviour>();
        p2Script = p2.GetComponent<PlayerBehaviour>();

        powerUps = new List<GameObject>();
        GameObject tmp = Instantiate(Resources.Load("Shield")) as GameObject;
        tmp.SetActive(false);
        tmp.transform.SetParent(GameObject.Find("Canvas").transform, false);
        powerUps.Add(tmp);
        players = GameObject.FindGameObjectsWithTag("Player");

        //GameObject.Find("ARCamera").GetComponent<CameraFlipper>().OnPreCull();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            float tmp = Random.Range(0.0f, 1.0f);

            if (tmp < probability && !powerUps[0].activeInHierarchy)
            {
                Debug.Log(tmp);
                powerUps[0].transform.position = new Vector3(Random.Range(200, 500), Random.Range(200, 500), 0);
                powerUps[0].SetActive(true);
            }

            for (int i = 0; i < powerUps.Count; i++)
                if (powerUps[i].activeInHierarchy)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(powerUps[i].transform.position), out hit, 500.0f))
                    {
                        hit.collider.gameObject.GetComponent<PlayerBehaviour>().addPowerUp(powerUps[i].name);
                        powerUps[i].SetActive(false);
                    }
                }
        }
        else if(waitingToStart) {
            TrackableBehaviour.Status p1Status = p1Script.getStatus();
            TrackableBehaviour.Status p2Status = p2Script.getStatus();

            if ((p1Status == TrackableBehaviour.Status.DETECTED ||
            p1Status == TrackableBehaviour.Status.TRACKED ||
            p1Status == TrackableBehaviour.Status.EXTENDED_TRACKED) && (p2Status == TrackableBehaviour.Status.DETECTED ||
            p2Status == TrackableBehaviour.Status.TRACKED ||
            p2Status == TrackableBehaviour.Status.EXTENDED_TRACKED)) {
                waitingToStart = false;
                gameActive = true;
                title.SetActive(false);
                instructions.SetActive(false);
            }
        }
    }

    public void GameOver(string name)
    {
        gameActive = false;
        for (int i = 0; i < powerUps.Count; i++)
            powerUps[i].SetActive(false);
        if (!over)
        {
            over = true;
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerBehaviour>().Cancel();

                if (!player.name.Contains(name))
                {
                    GameObject gameOver = Instantiate(Resources.Load("GameOver")) as GameObject;
                    gameOver.GetComponent<Text>().text = player.name + gameOver.GetComponent<Text>().text;
                    gameOver.transform.SetParent(transform, false);
                    gameOver.transform.position = new Vector3(512, 384);
                    foreach (Button child in gameOver.GetComponentsInChildren<Button>())
                        child.onClick.AddListener(() => Listener(child.name));
                }
            }
        }
    }

    void Listener(string name)
    {
        if (name.Contains("Play Again"))
        {
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerBehaviour>().resetHP();
                Destroy(GameObject.Find("GameOver(Clone)"));
                over = false;
            }
            waitingToStart = true;
            title.SetActive(true);
            instructions.SetActive(true);
        }
        else if (name.Contains("Exit"))
            Application.Quit();
    }
}
