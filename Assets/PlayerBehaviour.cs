using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {
    float BulletsPerSecond = 2;
    float RateOfFire;
	// Use this for initialization
	void Start () {
        RateOfFire = 1.0f / BulletsPerSecond;
        InvokeRepeating("Fire", 1.0f, RateOfFire);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Fire()
    {
        //TODO: Fire a missile (prefab in the folders)
    }
}
