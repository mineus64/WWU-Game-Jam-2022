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
    bool isStopped;
    [Header("Object Components")]
    [SerializeField] CharacterController characterController;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] public GameObject cameraAnchor;

    [SerializeField] Rigidbody rb;

    [Header("Movement Values")]
    [SerializeField] WalkState walkState;
    [SerializeField] Vector3 movement;
    [SerializeField] float baseMoveSpeed;
    [SerializeField] float moveSpeed;

    [Header("Sound Values")]
    [SerializeField] float footstepVolume;

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
            StartCoroutine(StepCountdown());
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
        Debug.Log(characterController.velocity.magnitude);
    }

    #endregion

    #region Specific Methods

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

    void SpawnSound() 
    {
        Sound currentSound = Instantiate(GameManager.current.sound, this.transform.position, Quaternion.identity).GetComponent<Sound>();

        currentSound.SetProperties(footstepVolume);
    }

    IEnumerator StepCountdown(){
        while(true){
            float stepTime = 0f;
            if (characterController.velocity.magnitude > 6){
                stepTime = .43f;
            } else if (characterController.velocity.magnitude > 4){
                stepTime = .59f;
            } else if (characterController.velocity.magnitude > 2){
                stepTime = 1.25f;
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