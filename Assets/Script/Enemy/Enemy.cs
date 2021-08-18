using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour,IDamagable
{
    [SerializeField] public EnemyScriptableObject m_enemyConfig;
    [SerializeField] private Animator enemyAnimator;
    
    private Rigidbody enemyRigidBody;

    private const float RotationSpeed = 6;
    private const float CollisionRadius = 5;

    [SerializeField] private float RotationAngleOnCollision = 90;

    private Coroutine enemyRotationCoroutine = null;
    private Coroutine playerCheckCoroutine;

    private Vector3 direction;
    private float rotation;

    private EnemyState currentState;

    private HealthController enemyhealthController;
    

    private Player player;

    [SerializeField] private Light spotLight;


    //public Enemy(EnemyScriptableObject enemyConfig)
    //{
    //    m_enemyConfig = enemyConfig;
    //    enemyhealthController = new HealthController(m_enemyConfig.InitialHealth);
    //}

    void Start()
    {
        enemyhealthController = new HealthController(m_enemyConfig.InitialHealth);
        enemyRigidBody = GetComponent<Rigidbody>();
        direction = new Vector3(0,0,m_enemyConfig.RoamingSpeed * Time.deltaTime);
        
        currentState = EnemyState.roam;
        enemyAnimator.Play("walk");
    }


    private void FixedUpdate()
    {
        if (currentState != EnemyState.chase)
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
                    rotation += RotationAngleOnCollision;
                    Quaternion temp = Quaternion.Euler(0, rotation, 0);
                    transform.rotation = Quaternion.Lerp(transform.rotation, temp, RotationSpeed);
                }
            }
            //will check collision every 1 sec
            yield return new WaitForSeconds(1);
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
                    OnPlayerDetected();
                }
            }

            yield return new WaitForSeconds(1);                 //will check every 1 sec for player
        }
    }

    private IEnumerator TurnToFace(Vector3 _lookTarget)
    {

        Vector3 _dirToLookTarget = (_lookTarget - transform.position).normalized;
        float _targetAngle = 90 - Mathf.Atan2(_dirToLookTarget.z, _dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, _targetAngle)) > 0.1f)
        {
            float _angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, _targetAngle, RotationSpeed);
            transform.eulerAngles = Vector3.up * _angle;
            yield return null;
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
                return true;
            }
        }
        return false;
    }

    private void OnPlayerDetected()
    {
        changeState(EnemyState.chase);

        StopCoroutine(enemyRotationCoroutine);
        enemyRotationCoroutine = null;

        StartCoroutine(TurnToFace(player.transform.position));

        spotLight.color = Color.red;
    }

    #endregion



    #region CHASING

    #endregion

    private void changeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }

    }

    public void OnDamage()
    {
        enemyhealthController.changeHealth(1);
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_enemyConfig.ChasingRadius);
    }
}
