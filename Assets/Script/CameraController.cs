using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    [SerializeField] private float movementSenstivity = 0.2f;
    [SerializeField] private float rotationSenstivity = 0.2f;



    Vector2 rotation;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position,target.position,movementSenstivity);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation,rotationSenstivity);

    }

}
