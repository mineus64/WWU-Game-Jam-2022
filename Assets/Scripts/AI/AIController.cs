//

using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 
public class AIController : MonoBehaviour
{
    #region Variables

    [Header("AI Actor Values")]
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;
    [SerializeField] UnityEngine.AI.NavMeshAgent navMeshAgent;
    [SerializeField] public Difficulty aiDifficulty;

    [Header("AI Activity")]
    [SerializeField] AIState currentState;

    [Header("Weapon Values")]
    [SerializeField] WeaponSlot[] weaponsInBag;
    [SerializeField] int currentWeapon = 0;
    [SerializeField] GameObject currentWeaponObj;
    [SerializeField] bool isFiring;

    [Header("AI Constraints")]
    [SerializeField] const bool canBecomeSelfAware = false;

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

    void LateUpdate() 
    {

    }

    void FixedUpdate() 
    {
        if (isServer) {
            currentState.OnStateTick();

            currentState.OnStateEvaluate();
        }
    }

    #endregion

    #region Specific Methods



    #endregion

    // This region contains methods and variables maintained for compatibility purposes, as other things depend on these
    // These should be updated in their respective scripts over time
    #region Compatibility

    // BulletObject.cs (33,61) depends on this
    public void DeltaHealth(float healthDelta) 
    {
        currentHealth += healthDelta;

        if (currentHealth <= 0) {
            Die();
        }
    }

    #endregion
}
