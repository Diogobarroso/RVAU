using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlipper : MonoBehaviour {

    private Camera myCam;
    private bool isFlipped = false;

	// Use this for initialization
	void Start () {
        isFlipped = false;
        
	}

    public void OnPreCull()
    {
        if (!isFlipped) {
            myCam = GetComponent<Camera>();
            Vector3 flip = new Vector3(-1, 1, 1);
            myCam.projectionMatrix = myCam.projectionMatrix * Matrix4x4.Scale(flip);
            isFlipped = true;
        }
        
    }

    // Update is called once per frame
    void Update () {        
    }
}
