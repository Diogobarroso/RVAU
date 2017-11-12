using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlipper : MonoBehaviour {

	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
        transform.GetChild(0).eulerAngles = new Vector3(-90, 180, 0);

    }
}
