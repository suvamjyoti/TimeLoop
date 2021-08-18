using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTargetController : MonoBehaviour
{
    [SerializeField]private float mouseSensititvity;
    private float x;
    

    private void Update()
    {
        x += Input.GetAxisRaw("Mouse X") * mouseSensititvity;
        
        if(x > -90 && x < 90)
        {
            transform.localRotation = Quaternion.Euler(0, x, 0);
        }

    }

}
