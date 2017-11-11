using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {
    float speed = 0.3f;
    public float dmg = 10;
    private void OnEnable()
    {
        Invoke("Destroy", 2.0f);
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }
    

    private void OnDisable()
    {
        CancelInvoke();
    }

    void Update()
    {
        //transform.Translate(Vector3.up * Time.deltaTime);
        GetComponent<Rigidbody>().velocity = Vector3.forward * speed;
    }
}
