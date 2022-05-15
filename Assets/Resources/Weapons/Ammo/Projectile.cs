using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Objects/Projectile")]
public class Projectile : ScriptableObject
{
    [Header ("Cosmetic")]
    public string projectileName;

    [Header("Projectile Object")]

    public GameObject weaponObject;

    [Header("Projectile Attributes")]

    public float flyingSpeed;
    public bool explodes;

    public bool penetrates;
    
}
