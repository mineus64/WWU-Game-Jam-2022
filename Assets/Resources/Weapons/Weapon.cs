using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : ScriptableObject
{
    #region Variables

    [Header("Cosmetic")]
    public string weaponName;
    public string weaponDesc;
    public Sprite weaponIcon;

    [Header("Weapon Object")]
    public GameObject weaponObject;

    [Header("Weapon Attributes")]
    public float fireRate;
    public int magSize;
    public int reloadTime;

    [Header("Sound")]
    public float weaponNoise;

    #endregion

    #region Specific Methods



    #endregion
}
