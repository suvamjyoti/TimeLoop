using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Animator playerAnimator;

    private playerState playerPreviousState = playerState.none;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        ChangePlayerState(playerState.ideal);
    }


    void Update()
    {

        float _translation = Input.GetAxisRaw("Vertical");
        float _rotation = Input.GetAxisRaw("Horizontal");

        if(_translation >0)
        {
            transform.Translate(_translation * speed * Time.deltaTime, 0, 0);
            ChangePlayerState(playerState.walk);
        }
        else
        {
            ChangePlayerState(playerState.ideal);
        }

        transform.Rotate(0, _rotation * rotationSpeed * Time.deltaTime, 0);
    }




    void ChangePlayerState(playerState newState)
    {
        if(newState != playerPreviousState)
        {
            switch (newState)
            {
                case playerState.walk:
                    playerAnimator.Play("walk");
                    break;
                case playerState.die:
                    playerAnimator.Play("die");
                    break;
                case playerState.ideal:
                    playerAnimator.Play("ideal");
                    break;
            }

            playerPreviousState = newState;
        }
    }
}
