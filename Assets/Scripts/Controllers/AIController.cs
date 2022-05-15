// This script should be attached to the AI prefab. It handles AI logic etc.

using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    [Header("AI")]
    [SerializeField] NavMeshAgent navMeshAgent;

    [Header("AI Behaviour Values")]
    [SerializeField] AIBehaviour behaviour = AIBehaviour.Patrolling;
    [SerializeField] PlayerController playerTarget;
    [SerializeField] Vector3 patrolTarget;
    [SerializeField] float AITimer;

    [Header("AI Constraints")]
    [SerializeField] bool canBecomeSelfAware = false;
    [SerializeField] Vector4 movementSampleConstraints = new Vector4(-15.0f, 15.0f, -15.0f, 15.0f);

    #endregion

    #region General Methods

    // Start is called before the first frame update
    void Start()
    {
        playerTarget = GameManager.current.currentClient;
        StartCoroutine(StepCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        AITimer = Mathf.Max(AITimer - Time.deltaTime, 0.0f);

        if (AITimer > 0) {
            switch (behaviour) 
            {
                case AIBehaviour.Patrolling:
                    PatrolBehaviour();
                    break;
                case AIBehaviour.Searching:
                    SearchBehaviour();
                    break;
                case AIBehaviour.Attacking:
                    AttackBehaviour();
                    break;
                case AIBehaviour.Retreating:
                    RetreatBehaviour();
                    break;
            }
        }
        else {
            switch (behaviour) 
            {
                case AIBehaviour.Patrolling:
                    PatrolExit();
                    break;
                case AIBehaviour.Searching:
                    SearchExit();
                    break;
                case AIBehaviour.Attacking:
                    AttackExit();
                    break;
                case AIBehaviour.Retreating:
                    RetreatExit();
                    break;
            }
        }
    }

    #endregion

    #region Specific Methods

    void PatrolEnter() 
    {
        patrolTarget = new Vector3(
            Random.Range(movementSampleConstraints.x, movementSampleConstraints.y),
            0.0f,
            Random.Range(movementSampleConstraints.z, movementSampleConstraints.w)
        );

        NavMeshHit hit;

        NavMesh.SamplePosition(patrolTarget, out hit, 2.0f, NavMesh.AllAreas);

        navMeshAgent.SetDestination(hit.position);
    }

    void PatrolBehaviour() 
    {
        RaycastHit hit = new RaycastHit();

        Vector3 rayDirection = playerTarget.transform.position - this.transform.position;

        if (Physics.Raycast(this.transform.position, rayDirection, out hit)) {
            if (hit.transform.GetComponent<PlayerController>() != null) {
                behaviour = AIBehaviour.Attacking;
            }
        }
    }

    void PatrolExit() 
    {
        float random = Random.Range(0, 1);

        if (random >= 0.5) {
            behaviour = AIBehaviour.Patrolling;

            AITimer = 10.0f;

            PatrolEnter();
        }
        else {
            behaviour = AIBehaviour.Searching;

            AITimer = 5.0f;

            SearchEnter();
        }
    }

    void SearchEnter() 
    {
        patrolTarget = playerTarget.transform.position;

        NavMeshHit hit;

        NavMesh.SamplePosition(patrolTarget, out hit, 2.0f, NavMesh.AllAreas);

        navMeshAgent.SetDestination(hit.position);
    }

    void SearchBehaviour() 
    {
        RaycastHit hit = new RaycastHit();

        Vector3 rayDirection = playerTarget.transform.position - this.transform.position;

        if (Physics.Raycast(this.transform.position, rayDirection, out hit)) {
            if (hit.transform.GetComponent<PlayerController>() != null) {
                behaviour = AIBehaviour.Attacking;
            }
        }
    }

    void SearchExit() 
    {
        float random = Random.Range(0, 1);

        if (random >= 0.5) {
            behaviour = AIBehaviour.Patrolling;

            AITimer = 10.0f;

            PatrolEnter();
        }
        else {
            behaviour = AIBehaviour.Searching;

            AITimer = 5.0f;

            SearchEnter();
        }
    }

    void AttackEnter() 
    {

    }

    void AttackBehaviour() 
    {
        if (currentHealth <= 20.0f) {
            behaviour = AIBehaviour.Retreating;
        }

        RaycastHit hit = new RaycastHit();

        Vector3 rayDirection = playerTarget.transform.position - this.transform.position;

        if (!Physics.Raycast(this.transform.position, rayDirection, out hit)) {
            behaviour = AIBehaviour.Searching;
        }
    }

    void AttackExit() 
    {

    }

    void RetreatEnter() 
    {
        patrolTarget = Vector3.Normalize(playerTarget.transform.position - this.transform.position);

        patrolTarget = Quaternion.Euler(0.0f, 180.0f, 0.0f) * patrolTarget;

        patrolTarget *= 20;

        NavMeshHit hit;

        NavMesh.SamplePosition(patrolTarget, out hit, 20.0f, NavMesh.AllAreas);

        navMeshAgent.SetDestination(hit.position);
    }

    void RetreatBehaviour() 
    {
        RaycastHit hit = new RaycastHit();

        Vector3 rayDirection = playerTarget.transform.position - this.transform.position;

        if (!Physics.Raycast(this.transform.position, rayDirection, out hit)) {
            currentHealth = Mathf.Min(currentHealth + Time.deltaTime, 50);
        }

        if (currentHealth >= 50) {
            RetreatExit();
        }
    }

    void RetreatExit() 
    {
        float random = Random.Range(0, 1);

        if (random >= 0.5) {
            behaviour = AIBehaviour.Patrolling;

            AITimer = 10.0f;

            PatrolEnter();
        }
        else {
            behaviour = AIBehaviour.Searching;

            AITimer = 5.0f;

            SearchEnter();
        }
    }

    IEnumerator StepCountdown(){
        while(true){
            float stepTime = stepLength*0f;

            stepTime = stepLength*.59f;
            
            float totalTime = 0f;
            while (totalTime < stepTime){
                totalTime += Time.deltaTime;
                yield return null;
            }
            SpawnSound();
            yield return null;
        }
    }

    void SpawnSound() 
    {
        Sound currentSound = Instantiate(GameManager.current.sound, this.transform.position, Quaternion.identity).GetComponent<Sound>();

        currentSound.SetProperties(footstepVolume);
    }

    #endregion
}

enum AIBehaviour 
{
    Patrolling,
    Searching,
    Attacking,
    Retreating
}