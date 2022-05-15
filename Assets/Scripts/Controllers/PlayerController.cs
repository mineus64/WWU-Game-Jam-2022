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

    [Header("Object Components")]
    [SerializeField] CharacterController characterController;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] public GameObject cameraAnchor;

    [Header("Movement Values")]
    [SerializeField] WalkState walkState;
    [SerializeField] Vector3 movement;
    [SerializeField] float baseMoveSpeed;
    [SerializeField] float moveSpeed;

    [Header("Sound Values")]
    [SerializeField] float footstepVolume;

    [Header("Weapon Values")]
    [SerializeField] bool[] weaponsInBag = 
    {
        true,       // Pistol
        false,      // Anti-Material Rifle
        false,      // Assault Rifle
        false,      // Big Bang Grenade
        false,      // Decoy Grenade
        false,      // Frag Grenade
        false,      // Pistol (Suppressed)
        false,      // Rocket Launcher
        false,      // Shotgun
        false       // SMG
    };

    #endregion

    #region General Methods

    void Awake() 
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {

        // Todo on all players
        if (characterController == null) {
            characterController = this.GetComponent<CharacterController>();
        }

        // Todo only if this player is the current client's player
        if (isLocalPlayer) {
            GameManager.current.currentClient = this;
        }

        // Todo only if this player is NOT the current client's player
        else {
            
            if (playerInput == null) {
                playerInput = this.GetComponent<PlayerInput>();
            }
            Destroy(playerInput);

        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.Normalize(movement);
        move = this.transform.TransformDirection(move);

        characterController.SimpleMove(move * moveSpeed);

    }

    void FixedUpdate() {

        // Spawn footstep objects
        if (movement != Vector3.zero) {
            float randValue = Random.Range(0, 1000);

            if (randValue <= moveSpeed) {
                SpawnSound();
            }
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

        switch (movement) 
        {
            case < 0:
                walkState = WalkState.Walking;
                break;
            case > 0:
                walkState = WalkState.Running;
                break;
            default:
                walkState = WalkState.Jogging;
                break;
        }
    }

    public void SwitchWeapon(InputAction.CallbackContext context) 
    {
        Vector2 rawInput = context.ReadValue<Vector2>();

        float refinedInput = rawInput.y;


    }

    #endregion

    #region Specific Methods

    void SpawnSound() 
    {
        Sound currentSound = Instantiate(GameManager.current.sound, this.transform.position, Quaternion.identity).GetComponent<Sound>();

        currentSound.SetProperties(footstepVolume);
    }

    void SetWeapon(int weaponSwitch = 0, int weaponSet = 0) 
    {
        if (weaponSwitch == 0 && weaponSet == 0) {
            return;
        }
        else {
            if (weaponSwitch != 0) {

            }

            if (weaponSet != 0) {

            }
        }
    }

    #endregion
}

#region Enums

public enum WalkState 
{
    Walking,
    Jogging,
    Running
}

#endregion