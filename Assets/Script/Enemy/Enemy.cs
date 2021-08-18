using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject m_enemyConfig;
    [SerializeField] private Animator enemyAnimator;
    
    private Rigidbody enemyRigidBody;

    private const float RotationSpeed = 6;
    private const float CollisionRadius = 5;
    private const float RotationAngle = 30;
    
    private Vector3 direction;
    private float rotation;

    //public Enemy(EnemyScriptableObject enemyConfig)
    //{
    //    m_enemyConfig = enemyConfig;
    //}

    void Start()
    {
        enemyRigidBody = GetComponent<Rigidbody>();
        //should move forward
        direction = new Vector3(m_enemyConfig.RoamingSpeed * Time.deltaTime, 0, 0);

        enemyAnimator.Play("walk");
    }


    private void FixedUpdate()
    {
        enemyRigidBody.transform.Translate(direction);

    }

    void Update()
    {
        StartCoroutine(enemyRotationCoroutine());
    }


    private IEnumerator enemyRotationCoroutine()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, CollisionRadius);

        foreach(Collider collider in hitColliders)
        {
            if(collider.tag == "obstacle")
            {
                rotation += RotationAngle;
                Quaternion temp = Quaternion.Euler(0, rotation, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation,temp, RotationSpeed);
            }
        }
        //will check collision every 2 sec
        yield return new WaitForSeconds(2);
    }

}
