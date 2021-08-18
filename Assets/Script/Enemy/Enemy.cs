using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour,IDamagable
{
    [SerializeField] public EnemyScriptableObject m_enemyConfig;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private LayerMask viewMask;

    private Rigidbody enemyRigidBody;

    private const float RotationSpeed = 6;
    private const float CollisionRadius = 5;
    private const float RotationAngle = 30;

    private Coroutine enemyRotationCoroutine = null;
    private Coroutine playerCheckCoroutine;

    private Vector3 direction;
    private float rotation;

    private EnemyState currentState;

    private HealthController enemyhealthController;
    

    private Player player;


    //public Enemy(EnemyScriptableObject enemyConfig)
    //{
    //    m_enemyConfig = enemyConfig;
    //    enemyhealthController = new HealthController(m_enemyConfig.InitialHealth);
    //}

    void Start()
    {
        enemyhealthController = new HealthController(m_enemyConfig.InitialHealth);
        enemyRigidBody = GetComponent<Rigidbody>();
        direction = new Vector3(m_enemyConfig.RoamingSpeed * Time.deltaTime, 0, 0);
        
        currentState = EnemyState.roam;
        enemyAnimator.Play("walk");
    }


    private void FixedUpdate()
    {
        if (currentState != EnemyState.die)
        {
            enemyRigidBody.transform.Translate(direction);
        }
    }

    void Update()
    {
        if (currentState==EnemyState.roam && enemyRotationCoroutine is null)
        {
            enemyRotationCoroutine = StartCoroutine(RotationCoroutine());
        }

        if (currentState != EnemyState.chase && playerCheckCoroutine is null)
        {
            playerCheckCoroutine = StartCoroutine(checkForPlayer());
        }

        if(currentState == EnemyState.chase)
        {
            CheckPlayerDistance();
        }
    }
    
    private void CheckPlayerDistance()
    {
        if(player != null)
        {
            float difference = Vector3.Distance(transform.position, player.transform.position);
            if (difference < 2)
            {
                currentState = EnemyState.die;
                enemyAnimator.Play("die");
            }
        }
    }

    private IEnumerator RotationCoroutine()
    {
        while (true)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, CollisionRadius);

            foreach (Collider collider in hitColliders)
            {
                if (collider.tag == "obstacle")
                {
                    rotation += RotationAngle;
                    Quaternion temp = Quaternion.Euler(0, rotation, 0);
                    transform.rotation = Quaternion.Lerp(transform.rotation, temp, RotationSpeed);
                }
            }
            //will check collision every 2 sec
            yield return new WaitForSeconds(2);
        }
    }

   

    public void OnDamage()
    {
        enemyhealthController.changeHealth(1);
    }

    private void changeState(EnemyState newState)
    {
        if(currentState != newState)
        {
            currentState = newState;
        }

    }

    #region DETECTION

    private IEnumerator checkForPlayer()
    {
        while(currentState == EnemyState.roam)
        {
            if (PlayerInProximity())
            {
                if (CanSeePlayer())
                {
                    changeState(EnemyState.chase);
                    
                    StopCoroutine(enemyRotationCoroutine);
                    enemyRotationCoroutine = null;

                    TurnToFace(player.transform.position);

                    //MoveTowardsPlayer();
                }
            }

            yield return new WaitForSeconds(2);                 //will check every 2 sec for player
        }
    }

    private IEnumerator TurnToFace(Vector3 _lookTarget)
    {

        Vector3 _dirToLookTarget = (_lookTarget - transform.position).normalized;
        float _targetAngle = 90 - Mathf.Atan2(_dirToLookTarget.z, _dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, _targetAngle)) > 0.5f)
        {
            float _angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, _targetAngle, RotationSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * _angle;
            yield return null;
        }
    }

    private void MoveTowardsPlayer()
    {
        if(player != null)
        {
            direction = (player.transform.position - transform.position).normalized * m_enemyConfig.ChasingSpeed;
        }
    }


    private bool PlayerInProximity()
    {
        var _collidersInRange = Physics.OverlapSphere(transform.position, m_enemyConfig.ChasingRadius);            //define a proximity Sphere

        foreach (var item in _collidersInRange)
        {
            if (item.tag == "Player")
            {                                                                       //if objects in sphere has tag player 
                player = item.GetComponent<Player>();
                return true;
            }
        }
        return false;
    }

    private bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < m_enemyConfig.ChasingRadius)
        {                                                                                                           //Is Player In View Range

            Vector3 _directionToPlayer = (player.transform.position - transform.position).normalized;
            float _angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, _directionToPlayer);

            if (_angleBetweenEnemyAndPlayer < m_enemyConfig.FieldOfView / 2f)
            {                                                                                                       //Is Player In View Cone

                //viewMask prevents enemy from seeing player beyond obstacle
                if (!Physics.Linecast(transform.position, player.transform.position, viewMask))
                {                                                                                                   //Is Player In Line Of Sight
                    return true;
                }
            }
        }
        return false;
    }

    #endregion


    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, transform.forward * m_enemyConfig.ChasingRadius);                         //ditection line range

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_enemyConfig.ChasingRadius);
    }
}
