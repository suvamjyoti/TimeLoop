using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    private float sensitivity = 10f;
    private Vector2 turn;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {

        float _translation = Input.GetAxisRaw("Vertical");
        float _rotation = Input.GetAxisRaw("Horizontal");

        turn.x = Input.GetAxis("Mouse X") * sensitivity;
        turn.y = Input.GetAxis("Mouse Y") * sensitivity;

        //transform.localRotation = Quaternion.Euler(0,-turn.y, 0);

        transform.Translate(_translation*speed*Time.deltaTime,0,0);
        //transform.Rotate(0, _rotation * rotationSpeed * Time.deltaTime, 0);
        transform.Rotate(0,turn.y, 0);
    }
}
