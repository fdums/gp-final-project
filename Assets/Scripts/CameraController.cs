using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offsetPosition;
    AudioSource audioSource;
    public AudioClip backgroundMusic;


    private void Start()
    {
        audioSource.PlayOneShot(backgroundMusic, 0.4F);
    }

    private void Update()
    {
        transform.position = target.TransformPoint(offsetPosition);
        transform.LookAt(target);

    }

}