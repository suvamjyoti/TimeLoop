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


    [SerializeField] private Transform gameOver;

    private Coroutine shootingCoroutine = null;
    [SerializeField] private Transform shootTransform;
    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private float attackRadius = 50;
    [SerializeField] private float timeGap;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        ChangePlayerState(playerState.ideal);
        playerHealthController = new HealthController(playerHealth,OnDeath);
    }


    void Update()
    {
        PlayerMovement();

        if (Input.GetButton("Fire1"))
        {
            if (shootingCoroutine is null)
            {
                shootingCoroutine = StartCoroutine(Shoot());
            }
        }
    }


    private void PlayerMovement()
    {
        float _translation = Input.GetAxisRaw("Vertical");
        float _rotation = Input.GetAxisRaw("Horizontal");

        if (_translation != 0)
        {
            transform.Translate(0, 0, _translation * speed * Time.deltaTime);
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


    private IEnumerator Shoot()
    {
        Rigidbody _obj = Instantiate(bulletPrefab, shootTransform) as Rigidbody;
        _obj.velocity = transform.forward * attackRadius;

        yield return new WaitForSeconds(timeGap);

        shootingCoroutine = null;
    }

    public void OnDeath()
    {
        ChangePlayerState(playerState.die);
        gameOver.gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(deathEffect());
    }

    private IEnumerator deathEffect()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
