using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {
    float speed = 0.3f;
    public float dmg = 10;

    private Vector3 moveDirection;

    private Rigidbody myRigidBody;

    private GameObject target;

    private Transform myTransform;

    private Quaternion originalQuart;

    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        myTransform = GetComponent<Transform>();

        originalQuart = myTransform.rotation;
    }

    private void OnEnable()
    {
        Invoke("Destroy", 3.0f);
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }
    

    private void OnDisable()
    {
        CancelInvoke();
    }


    public void setDirection(Vector3 direction) {
        moveDirection = direction;
       
        myTransform.rotation = Quaternion.LookRotation(direction);
        myTransform.rotation *= originalQuart ;
    }

    public void setTarget(GameObject enemyShip) {
        target = enemyShip;
    }

    void Update()
    {
        //transform.Translate(Vector3.up * Time.deltaTime);
        //GetComponent<Rigidbody>().velocity = Vector3.forward * speed;
        myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, target.transform.position.z);
        myRigidBody.velocity = moveDirection * speed;
    }

    public void setPlayer(bool player)
    {
        if (player)
            Physics.IgnoreLayerCollision(8, 8, true);
        else
            Physics.IgnoreLayerCollision(9, 9, true);
    }
}
