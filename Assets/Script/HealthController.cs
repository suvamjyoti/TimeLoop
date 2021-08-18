using System;
using UnityEngine;

public class HealthController : MonoBehaviour
{

    private int m_health;
    private Action onZeroHealth;


    public HealthController(int health, Action onDeathCallback)
    {
        m_health = health;
        onZeroHealth = onDeathCallback;
    }

    public virtual void changeHealth(int damage)
    {
        if (damage < m_health)
        {
            m_health -= damage;
            return;
        }

        //if damage higher then current health 
        m_health = 0;
        onZeroHealth.Invoke();
    }
}
