using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<IDamagable>() != null)
        {
            IDamagable obj = collider.GetComponent<IDamagable>();
            obj.OnDamage();
        }

        Destroy(gameObject);
    }

}
