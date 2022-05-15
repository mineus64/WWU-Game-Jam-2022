using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Objects/Weapon")]
public class Weapon : ScriptableObject
{
    #region Variables

    [Header("Cosmetic")]
    public string weaponName;
    [TextArea] public string weaponDesc;
    public string weaponNoiseCosmetic;
    public Sprite weaponIcon;

    [Header("Weapon Object")]
    public GameObject weaponObject;

    public GameObject weaponProjectile;

    [Header("Weapon Attributes")]
    public float fireRate;      // RPS
    public int magSize;
    public int maxAmmo;
    public float reloadTime;

    [Header("Sound")]
    public float weaponNoise;

    [Header("Offensive Stats")]
    public float damage;
    public float spread;        // Degrees

    public bool penetrates;

    public bool explodes;

    #endregion

    #region Specific Methods


    #endregion
}
