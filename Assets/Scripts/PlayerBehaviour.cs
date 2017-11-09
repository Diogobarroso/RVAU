using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class PlayerBehaviour : MonoBehaviour, ITrackableEventHandler {
    float bulletsPerSecond = 2;
    float rateOfFire;
    TrackableBehaviour myTrackableBehaviour;
    List<GameObject> bullets;

	void Start () {

        myTrackableBehaviour = GetComponent<TrackableBehaviour>();
        GameObject tmpBullet;
        for(int i = 0; i < 50; i++)
        {
            bullets.Add(Instantiate(Resources.Load("Missile")) as GameObject);
            bullets[i].transform.SetParent(transform);
            bullets[i].SetActive(false);
        }

        if (myTrackableBehaviour)
        {
            Debug.Log("tracked");
            myTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        rateOfFire = 1.0f / bulletsPerSecond;   
	}
	
	void Update () {
		
	}

    void Fire()
    {
        Debug.Log("FIRE");
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
}
