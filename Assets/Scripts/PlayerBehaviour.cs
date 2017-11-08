using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {
    float bulletsPerSecond = 3;
    float rateOfFire;
    float thrust = 0.000000001f;

	// Use this for initialization
	void Start ()
    {
        rateOfFire = 1.0f / bulletsPerSecond;
        InvokeRepeating("Fire", 1.0f, 0.3f);
    }
	
	// Update is called once per frame
	void Update ()
    {
	}

    void Fire()
    {
        GameObject bul = Instantiate(Resources.Load("Bullet"), transform.position, transform.rotation, transform) as GameObject;
        bul.GetComponent<Rigidbody2D>().AddForce(transform.right * thrust);
    }
}
