using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableWeapon", menuName = "ScriptableObject/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public WeaponType weaponType;
    public int Damage;
    public float TimeGap;
    public float AttackRadius;
    
}