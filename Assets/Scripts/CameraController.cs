using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offsetPosition;

    private void Update()
    {
        transform.position = target.TransformPoint(offsetPosition);
        transform.LookAt(target);

    }

}