using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    List<GameObject> powerUps;
    List<GameObject> activePowerUps;
    float probability = 0.02f; //Probability [0,1] with which a power up will spawn
                               // Use this for initialization
    void Start()
    {
        powerUps = new List<GameObject>();
        GameObject tmp = Instantiate(Resources.Load("Shield")) as GameObject;
        tmp.SetActive(false);
        tmp.transform.SetParent(GameObject.Find("Canvas").transform, false);
        powerUps.Add(tmp);

    }

    // Update is called once per frame
    void Update()
    {
        float tmp = Random.Range(0.0f, 1.0f);

        if (tmp < probability && !powerUps[0].activeInHierarchy)
        {
            Debug.Log(tmp);
            powerUps[0].transform.position = new Vector3(Random.Range(200, 500), Random.Range(200, 500), 0);
            powerUps[0].SetActive(true);
            //Invoke("RemovePowerUp", 5.0f);
        }

        for (int i = 0; i < powerUps.Count; i++)
            if (powerUps[i].activeInHierarchy)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(powerUps[i].transform.position), out hit,500.0f))
                {
                    hit.collider.gameObject.GetComponent<PlayerBehaviour>().addPowerUp(powerUps[i].name);
                    powerUps[i].SetActive(false);
                }
            }
    }

    public void GameOver(string name)
    {
        Debug.Log("gameover");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
            if (!player.name.Contains(name))
            {
                GameObject gameOver = Instantiate(Resources.Load("GameOver")) as GameObject;
                gameOver.GetComponent<GUIText>().text = player.name + gameOver.GetComponent<GUIText>().text;
                gameOver.transform.SetParent(transform);

            }
    }
}
