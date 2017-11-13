using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    List<GameObject> powerUps;
    List<GameObject> activePowerUps;
    float probability = 0.02f; //Probability [0,1] with which a power up will spawn
    bool over = false;
    GameObject[] players;
    bool gameActive = true;


    void Start()
    {
        powerUps = new List<GameObject>();
        GameObject tmp = Instantiate(Resources.Load("Shield")) as GameObject;
        tmp.SetActive(false);
        tmp.transform.SetParent(GameObject.Find("Canvas").transform, false);
        powerUps.Add(tmp);
        players = GameObject.FindGameObjectsWithTag("Player");

        GameObject.Find("ARCamera").GetComponent<CameraFlipper>().OnPreCull();
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
        }
        else if (name.Contains("Exit"))
            Application.Quit();
    }
}
