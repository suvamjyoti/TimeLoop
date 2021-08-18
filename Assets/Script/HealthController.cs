using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{

    private int m_health; 


    public HealthController(int health)
    {
        m_health = health;
    }

    public virtual void changeHealth(int damage)
    {
        if (damage <= m_health)
        {
            m_health -= damage;
            return;
        }

        //if damage higher then current health 
        m_health = 0;
    }
}
