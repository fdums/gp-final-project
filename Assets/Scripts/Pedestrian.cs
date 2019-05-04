using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{

    [SerializeField]
    public GameObject leftBoundary;
    public GameObject rightBoundary;
    public char axis;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
    }

    protected void CheckForCollision()
    {
        
    }

    protected void Walk()
    {
        Vector3 movement; 
        switch (axis)
        {
            case 'x':
                movement = new Vector3(0.3f, 0, 0);
                transform.position += movement;
                break;
            case 'y':
                movement = new Vector3( 0, 1.0f, 0);
                transform.position = movement;
                break;
            case 'z':
                movement = new Vector3( 0, 0, 1.0f);
                transform.position = movement;
                break;
        }



    }
}
