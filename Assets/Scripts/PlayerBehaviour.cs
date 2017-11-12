using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class PlayerBehaviour : MonoBehaviour, ITrackableEventHandler {
    GameObject shield;
    public Slider hp;
    float bulletsPerSecond = 2;
    float rateOfFire;
    TrackableBehaviour myTrackableBehaviour;
    List<GameObject> bullets;
    GameObject bullet;
    int poolSize = 10;
    bool bulletSide = true;//represents the side we will shoot the bullet from, true for right, false for left

    private bool cheating = false;

    [SerializeField]
    private GameObject shipNose;

    [SerializeField]
    private GameObject enemyShip;

    [SerializeField]
    private GameObject gameManager;

    [SerializeField]
    private bool player1;
    
    private Transform shipNoseTransform;

	void Start () {
        hp.value = hp.maxValue;
        bullets = new List<GameObject>();
        shipNoseTransform = shipNose.GetComponent<Transform>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(Resources.Load("Bullet"));
            obj.SetActive(false);
            //obj.GetComponent<BulletBehaviour>().setPlayer(player1);
            Physics.IgnoreCollision(obj.GetComponent<Collider>(), GetComponent<Collider>());
            bullets.Add(obj);
        }

        myTrackableBehaviour = GetComponent<TrackableBehaviour>();
            
        if (myTrackableBehaviour)
        {
            Debug.Log("tracked");
            myTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        rateOfFire = 1.0f / bulletsPerSecond;

        foreach (Transform child in transform)
            if (child.name.Contains("Shield"))
                shield = child.gameObject;

        shield.SetActive(false);
	}
	
	void Update () {
	}

    void Fire()
    {
        Debug.Log("FIRE");
        for (int i = 0; i < bullets.Count; i++)
            if (!bullets[i].activeInHierarchy)
            {
                bullets[i].transform.position = transform.position;
                Vector3 direction = shipNoseTransform.position - transform.position;
                direction.z = 0;
                direction = direction.normalized; 
                bullets[i].transform.localScale = new Vector3(0.0005f,0.0005f,0.0005f);
                //bullets[i].transform.SetParent(GameObject.Find("Empty").transform);
                //bullets[i].transform.rotation = Quaternion.identity;
                bullets[i].SetActive(true);
                bullets[i].GetComponent<BulletBehaviour>().setDirection(direction);
                bullets[i].GetComponent<BulletBehaviour>().setTarget(enemyShip);
                break;
            }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        Debug.Log("tracbalb");
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            if (cheating) {
                StopCoroutine(PunishPlayer());
                cheating = false;
            }
            InvokeRepeating("Fire", 1.0f, rateOfFire);
        }

        /*else if ( newStatus == TrackableBehaviour.Status.NOT_FOUND ) {
            if (gameManager.GetComponent<GameManager>().GetGameStart()) {
                StartCoroutine(PunishPlayer());
                cheating = true;
            }
        }*/
        else { 
            CancelInvoke();
        }
            

    }

    IEnumerator PunishPlayer() {
        yield return new WaitForSeconds(0.1f);
        hp.value -= 1;

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I was hit by a " + other.gameObject.name);
        if (!shield.activeInHierarchy)
            if (hp.value - other.gameObject.GetComponent<BulletBehaviour>().dmg > 0)
            {
                hp.value -= other.gameObject.GetComponent<BulletBehaviour>().dmg;
            }
            else {
                gameManager.GetComponent<GameManager>().GameOver(transform.name);
            }
                
    }

    public void addPowerUp(string power)
    {
        if (power.Contains("Shield"))
        {
            shield.SetActive(true);
            StartCoroutine(DestroyShield());
        }
    }

    IEnumerator DestroyShield()
    {
        yield return new WaitForSeconds(2);
        shield.SetActive(false);
    }
}
