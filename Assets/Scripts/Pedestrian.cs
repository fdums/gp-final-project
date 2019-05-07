using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{

    Vector3 movement = new Vector3(0.05f, 0, 0);


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
    }



   void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LeftPedCol"))
        {
            movement = movement * -1;
            Vector3 currentRotation = gameObject.transform.eulerAngles;
            transform.rotation = Quaternion.Euler(0, currentRotation.y * -1.0f, 0 );
        }
     }

    protected void Walk()
    {

        transform.position += movement;

    }



}
