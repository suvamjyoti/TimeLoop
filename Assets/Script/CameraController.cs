using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 rotationOffset;



    private void Update()
    {
        transform.position = target.transform.position + offset;
        transform.rotation = Quaternion.Euler(target.transform.localEulerAngles + rotationOffset);
    }

}
