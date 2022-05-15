// This script should be attached to the player prefab. It handles player inputs and controls.

using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    #region Variables



    #endregion

    #region General Methods

    void Awake() 
    {
        if (isLocalPlayer) {

        }
        else {

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



    #endregion
}
