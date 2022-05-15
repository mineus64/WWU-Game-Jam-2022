// This script should be attached to the player prefab. It handles player inputs and controls.

using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    #region Variables
    public bool allowfire;
    public float fireRate;

    [Header("Variables")]

    [Header("Object Components")]
    [SerializeField] CharacterController characterController;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] public GameObject cameraAnchor;
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

    void Awake() 
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        allowfire = true;
        // Todo on all players
        if (characterController == null) {
            characterController = this.GetComponent<CharacterController>();
        }

        // Todo only if this player is the current client's player
        if (isLocalPlayer) {
            GameManager.current.currentClient = this;
            StartCoroutine(StepCountdown());
        }

        // Todo only if this player is NOT the current client's player
        else {
            
            if (playerInput == null) {
                playerInput = this.GetComponent<PlayerInput>();
            }
            Destroy(playerInput);

        }

        currentWeaponObj = Instantiate(GameManager.current.weapons[currentWeapon].weaponObject, weaponAnchor.transform);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.Normalize(movement);
        move = this.transform.TransformDirection(move);

        characterController.SimpleMove(move * moveSpeed);
                // Temporary fix, because GameUIManager.current doesn't populate until after this class' Start() method is run. Will need to fix later.
        GameUIManager.current.UpdateMagAmmoCount(weaponsInBag[currentWeapon].magAmmo, weaponsInBag[currentWeapon].weapon.magSize);
        GameUIManager.current.UpdateResAmmoCount(weaponsInBag[currentWeapon].reserveAmmo, weaponsInBag[currentWeapon].weapon.maxAmmo);
        GameUIManager.current.UpdateWeaponIcon(weaponsInBag[currentWeapon].weapon.weaponIcon);

        if (isLocalPlayer) {
            GameUIManager.current.UpdateHealth(currentHealth, maxHealth);   // DEBUG
        }
    }
    void FixedUpdate() {
        if(isFiring && allowfire){
            Debug.Log("Firing!");
            allowfire = false;
            StartCoroutine(fireWeapon());
        }
    }

    #endregion

    #region Input Handlers

    public void MoveForwardBack(InputAction.CallbackContext context) 
    {
        float moveAmt = context.ReadValue<float>();

        movement.z = moveAmt;
    }

    public void StrafeLeftRight(InputAction.CallbackContext context) 
    {
        float moveAmt = context.ReadValue<float>();

        movement.x = moveAmt;
    }

    public void SprintWalk(InputAction.CallbackContext context) 
    {
        float movement = context.ReadValue<float>();

        moveSpeed = baseMoveSpeed + (movement * (baseMoveSpeed * 0.5f));

    }

    public void SwitchWeapon(InputAction.CallbackContext context) 
    {
        if (context.performed) {
            Vector2 rawInput = context.ReadValue<Vector2>();

            int refinedInput = (int)Mathf.Clamp(rawInput.y, -1.0f, 1.0f);

            SetWeapon(refinedInput);
        }
    }
    public void Fire(InputAction.CallbackContext context) 
    {
        if (context.started) {
            isFiring = true;
        }
        else if (context.performed) {
            isFiring = true;
        }
        else if (context.canceled) {
            isFiring = false;
        }
    }

    public void FireWeapon(InputAction.CallbackContext context){


    }

    #endregion

    #region Specific Methods

    void SpawnSound() 
    {
        Sound currentSound = Instantiate(GameManager.current.sound, this.transform.position, Quaternion.identity).GetComponent<Sound>();

        currentSound.SetProperties(footstepVolume);
    }

    IEnumerator StepCountdown(){
        while(true){
            float stepTime = stepLength*0f;
            if (characterController.velocity.magnitude > 6){
                stepTime = stepLength*.43f;
            } else if (characterController.velocity.magnitude > 4){
                stepTime = stepLength*.59f;
            } else if (characterController.velocity.magnitude > 2){
                stepTime = stepLength*1.25f;
            } else{
                yield return null;
                continue;
            }
            float totalTime = 0f;
            while (totalTime < stepTime){
                totalTime += Time.deltaTime;
                yield return null;
            }
            SpawnSound();
            yield return null;
        }
    }

    IEnumerator FireCooldown(float rateOfFire){
        new WaitForSeconds(1/rateOfFire);
        yield return null;
    }

    void SetWeapon(int weaponSwitch = 0, int weaponSet = 0) 
    {

        if (weaponSwitch == 0 && weaponSet == 0) {
            return;
        }

        else {
            if (weaponSwitch != 0) {
                currentWeapon += weaponSwitch;

                if (currentWeapon >= weaponsInBag.Length) {
                    currentWeapon = 0;
                }
                else if (currentWeapon < 0) {
                    currentWeapon = weaponsInBag.Length - 1;
                }
            }

            if (weaponSet != 0) {
                currentWeapon = weaponSet;
            }
        }
        // Checking for if the selected weapon is valid. This is a clunky way of doing it, admittedly, but it will do for now.
        while (weaponsInBag[currentWeapon].isInBag == false) {
            if (weaponSwitch >= 0) {
                currentWeapon += 1;

                if (currentWeapon >= weaponsInBag.Length) {
                    currentWeapon = 0;
                }
            }
            else if (weaponSwitch < 0) {
                currentWeapon -= 1;

                if (currentWeapon < 0) {
                    currentWeapon = weaponsInBag.Length - 1;
                }
            }

        }

        Destroy(currentWeaponObj);

        currentWeaponObj = Instantiate(GameManager.current.weapons[currentWeapon].weaponObject, weaponAnchor.transform);

        GameUIManager.current.UpdateMagAmmoCount(weaponsInBag[currentWeapon].magAmmo, weaponsInBag[currentWeapon].weapon.magSize);
        GameUIManager.current.UpdateResAmmoCount(weaponsInBag[currentWeapon].reserveAmmo, weaponsInBag[currentWeapon].weapon.maxAmmo);
        GameUIManager.current.UpdateWeaponIcon(weaponsInBag[currentWeapon].weapon.weaponIcon);
    }

    IEnumerator fireWeapon(){
        fireRate = GameManager.current.weapons[currentWeapon].fireRate;
        float originalFireRate = fireRate;
        while(fireRate > 0 && isFiring == true){
            fireRate -= 1;
            GameObject bullet = Instantiate(GameManager.current.weapons[currentWeapon].weaponProjectile, weaponAnchor.transform.position, Quaternion.identity);
            bullet.GetComponent<Transform>().SetParent(this.transform);
            bullet.transform.rotation = Quaternion.Euler(90,0,this.transform.rotation.y);
            bullet.GetComponent<Transform>().SetParent(null, true);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * GameManager.current.weapons[currentWeapon].weaponProjectile.GetComponent<BulletObject>().FiringSpeed * 300);
            yield return new WaitForSeconds(1/originalFireRate);
        }
        allowfire = true;
        yield return null;
    }
    public void AddWeapon(int weapon, int ammo = 0) 
    {
        WeaponSlot weaponSlot = weaponsInBag[weapon];

        if (weaponSlot.isInBag == false) {
            weaponSlot.isInBag = true;

            weaponSlot.magAmmo = Mathf.Min(weaponSlot.weapon.magSize, ammo);

            ammo -= weaponSlot.magAmmo;

            GameUIManager.current.WeaponPickup(weapon);
        }
        else {
            GameUIManager.current.AmmoPickup(weapon);
        }

        weaponSlot.reserveAmmo = Mathf.Min(weaponSlot.reserveAmmo + ammo, weaponSlot.weapon.maxAmmo);
    }

    public void RemoveWeapon(int weapon) 
    {
        weaponsInBag[weapon].isInBag = false;
    }

    public void SetHealth(float healthSet) 
    {
        currentHealth = healthSet;

        if (isLocalPlayer) {
            GameUIManager.current.UpdateHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0) {
            Die();
        }
    }

    public void DeltaHealth(float healthDelta) 
    {
        currentHealth += healthDelta;

        if (isLocalPlayer) {
            GameUIManager.current.UpdateHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Die() 
    {

    }

    #endregion
}
#region Enums

public enum WalkState 
{
    Walking,
    Jogging,
    Running,
    Stopped
}

#endregion

#region WeaponSlot

[System.Serializable]
public class WeaponSlot 
{
    #region Variables

    public string name;
    public bool isInBag;
    public int magAmmo;
    public int reserveAmmo;

    public Weapon weapon;

    #endregion
}

#endregion
