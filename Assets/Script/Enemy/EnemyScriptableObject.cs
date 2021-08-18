using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ScriptableEnemy", menuName = "ScriptableObject/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public EnemyType enemyType;
    public float RoamingSpeed;
    public int InitialHealth;
    public float ChasingSpeed;
    public float ChasingRadius;
    public float FieldOfView;
    public WeaponScriptableObject weapon;
}
