// This script should be attached to the AI prefab. It handles AI logic etc.

using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AIController : NetworkBehaviour
{
    #region Variables

    [Header("Object Components")]
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject weaponAnchor;

    [Header("Movement Values")]
    [SerializeField] Vector3 movement;
    [SerializeField] float baseMoveSpeed;
    [SerializeField] float moveSpeed;

    [Header("Sound Values")]
    [SerializeField] float footstepVolume;
    [SerializeField] float stepLength = 1f;

    [Header("Weapon Values")]
    [SerializeField] WeaponSlot[] weaponsInBag;
    [SerializeField] int currentWeapon = 0;
    [SerializeField] GameObject currentWeaponObj;
    [SerializeField] bool isFiring;

    [Header("Health Values")]
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;

    #endregion

    #region General Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Specific Methods



    #endregion
}
