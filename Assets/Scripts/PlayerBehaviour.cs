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

	void Start () {
        hp.value = hp.maxValue;
        bullets = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(Resources.Load("Bullet"));
            obj.SetActive(false);
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
                bullets[i].transform.localScale = new Vector3(0.0005f,0.0005f,0.0005f);
                //bullets[i].transform.SetParent(GameObject.Find("Empty").transform);
                //bullets[i].transform.rotation = Quaternion.identity;
                bullets[i].SetActive(true);
                break;
            }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        Debug.Log("tracbalb");
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            InvokeRepeating("Fire", 1.0f, rateOfFire);
        else
            CancelInvoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!shield.activeInHierarchy && hp.value - other.gameObject.GetComponent<BulletBehaviour>().dmg > 0)
            hp.value -= other.gameObject.GetComponent<BulletBehaviour>().dmg;
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
