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
        
        if(x > 0 && x < 180)
        {
            transform.localRotation = Quaternion.Euler(0, x, 0);
        }

    }

}
