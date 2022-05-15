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

    #endregion

    #region General Methods

    void Awake() 
    {
        // Todo on all players
        if (characterController == null) {
            characterController = this.GetComponent<CharacterController>();
        }

        // Todo only if this player is the current client's player
        if (isLocalPlayer) {

        }

        // Todo only if this player is NOT the current client's player
        else {

            if (playerInput == null) {
                playerInput = this.GetComponent<PlayerInput>();
            }
            Destroy(playerInput);

        }
    }

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

    public void MoveForwardBack(InputAction.CallbackContext context) 
    {
        float moveAmt = context.ReadValue<float>();

        characterController.Move(Vector3.forward * moveAmt * Time.deltaTime);
    }

    public void StrafeLeftRight(InputAction.CallbackContext context) 
    {
        float moveAmt = context.ReadValue<float>();

        characterController.Move(Vector3.right * moveAmt * Time.deltaTime);
    }

    #endregion
}
