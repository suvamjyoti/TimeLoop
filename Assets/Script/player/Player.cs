using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IDamagable
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Animator playerAnimator;
    
    private playerState playerPreviousState = playerState.none;

    private HealthController playerHealthController;

    [SerializeField] private int playerHealth;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        ChangePlayerState(playerState.ideal);
        playerHealthController = new HealthController(playerHealth);
    }


    void Update()
    {
        PlayerMovement();
    }


    private void PlayerMovement()
    {
        float _translation = Input.GetAxisRaw("Vertical");
        float _rotation = Input.GetAxisRaw("Horizontal");

        if (_translation > 0)
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

    private void ChangePlayerState(playerState newState)
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

    public void OnDamage()
    {
        playerHealthController.changeHealth(1);
    }
}
