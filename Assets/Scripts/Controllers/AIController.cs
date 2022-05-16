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
    public Vector3 currentPos;
    public Vector3 lastPos;
    public bool allowfire;

    public float fireRate;

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
    [SerializeField] public Difficulty aiDifficulty;

    [Header("AI Behaviour Values")]
    [SerializeField] AIBehaviour behaviour = AIBehaviour.Patrolling;
    [SerializeField] PlayerController playerTarget;
    [SerializeField] Vector3 patrolTarget;
    [SerializeField] float AITimer;

    [Header("AI Constraints")]
    [SerializeField] bool canBecomeSelfAware = true;
    [SerializeField] Vector4 movementSampleConstraints = new Vector4(-15.0f, 15.0f, -15.0f, 15.0f);

    [Header("Timers")]
    [SerializeField] float deathTimer = 0.0f;
    [SerializeField] bool dead = false;

    #endregion

    #region General Methods

    // Start is called before the first frame update
    void Start()
    {
        playerTarget = GameManager.current.currentClient;
        StartCoroutine(StepCountdown());
        currentWeaponObj = Instantiate(GameManager.current.weapons[2].weaponObject, weaponAnchor.transform);
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

        deathTimer = Mathf.Max(deathTimer - Time.deltaTime, 0.0f);

        if (deathTimer == 0 && dead == true) {
            GameManager.current.Respawn(this.gameObject);

            behaviour = AIBehaviour.Patrolling;
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
            if (hit.transform == playerTarget.gameObject.transform) {
                behaviour = AIBehaviour.Attacking;
            }
        }
    }

    void PatrolExit() 
    {
        float random = Random.Range(0.0f, 1.0f);

        if (random >= DifficultyBehaviorMod(aiDifficulty)) {
            behaviour = AIBehaviour.Patrolling;

            AITimer = 5.0f;

            PatrolEnter();
        }
        else {
            behaviour = AIBehaviour.Searching;

            AITimer = 2.5f;

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
            if (hit.transform == playerTarget.gameObject.transform) {
                behaviour = AIBehaviour.Attacking;
            }
        }
    }

    void SearchExit() 
    {
        float random = Random.Range(0.0f, 1.0f);

        if (random >= DifficultyBehaviorMod(aiDifficulty)) {
            behaviour = AIBehaviour.Patrolling;

            AITimer = 5.0f;

            PatrolEnter();
        }
        else {
            behaviour = AIBehaviour.Searching;

            AITimer = 2.5f;

            SearchEnter();
        }
    }

    void AttackEnter() 
    {
        patrolTarget = playerTarget.transform.position;

        NavMeshHit hit;

        NavMesh.SamplePosition(patrolTarget, out hit, 2.0f, NavMesh.AllAreas);

        navMeshAgent.SetDestination(hit.position);
    }

    void AttackBehaviour() 
    {
        transform.LookAt(playerTarget.transform);

        if (currentHealth <= 20.0f) {
            behaviour = AIBehaviour.Retreating;
        }
        transform.LookAt(playerTarget.transform);
        float random = Random.Range(0.0f, 1.0f);
        if( (random < 0.8f) && allowfire){
            Debug.Log("Firing!");
            allowfire = false;
            StartCoroutine(fireWeapon());
        }
        RaycastHit hit = new RaycastHit();

        Vector3 rayDirection = playerTarget.transform.position - this.transform.position;

        if (Physics.Raycast(this.transform.position, rayDirection, out hit)) {
            if (hit.transform != playerTarget.gameObject.transform) {
                behaviour = AIBehaviour.Searching;

                AITimer = 2.5f;
            }
        }

        if (Vector3.Distance(playerTarget.transform.position, this.transform.position) > 10.0f) {
            patrolTarget = playerTarget.transform.position;

            NavMeshHit navhit;

            NavMesh.SamplePosition(patrolTarget, out navhit, 2.0f, NavMesh.AllAreas);

            navMeshAgent.SetDestination(navhit.position);
        }
        else {
            navMeshAgent.SetDestination(this.transform.position);
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
        float random = Random.Range(0.0f, 1.0f);

        if (random >= DifficultyBehaviorMod(aiDifficulty)) {
            behaviour = AIBehaviour.Patrolling;

            AITimer = 5.0f;

            PatrolEnter();
        }
        else {
            behaviour = AIBehaviour.Searching;

            AITimer = 2.5f;

            SearchEnter();
        }
    }

    IEnumerator StepCountdown(){
        while(true){
            float stepTime = stepLength*0f;
            currentPos = this.transform.position;
            if (Mathf.Abs((currentPos - lastPos).magnitude) > 1) {
                stepTime = stepLength*.59f;
            } else {
                yield return null; 
                continue;
            }

            float totalTime = 0f;
            while (totalTime < stepTime){
                totalTime += Time.deltaTime;
                lastPos = currentPos;
                yield return null;
            }
            SpawnSound();
            yield return null;
        }
    }
    IEnumerator fireWeapon(){
        fireRate = GameManager.current.weapons[2].fireRate;
        float originalFireRate = fireRate;
        while(fireRate > 0 && isFiring == true){
            fireRate -= 1;
            GameObject bullet = Instantiate(GameManager.current.weapons[2].weaponProjectile, weaponAnchor.transform.position, Quaternion.identity);
            if(this.gameObject.CompareTag("Player")){
                bullet.GetComponent<BulletObject>().IsPlayerBullet = true;
            } else{
                bullet.GetComponent<BulletObject>().IsPlayerBullet = false;
            }
            bullet.GetComponent<BulletObject>().Damage = GameManager.current.weapons[2].damage;
            bullet.GetComponent<Transform>().SetParent(this.transform);
            bullet.transform.rotation = Quaternion.Euler(90,0,this.transform.rotation.y);
            bullet.GetComponent<Transform>().SetParent(null, true);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * GameManager.current.weapons[2].weaponProjectile.GetComponent<BulletObject>().FiringSpeed * 300);
            yield return new WaitForSeconds(1/originalFireRate);
        }
        allowfire = true;
        yield return null;
    }

    void SpawnSound() 
    {
        Sound currentSound = Instantiate(GameManager.current.sound, this.transform.position, Quaternion.identity).GetComponent<Sound>();

        currentSound.SetProperties(footstepVolume);
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
        behaviour = AIBehaviour.Dead;

        dead = true;

        deathTimer = 2.5f;

        GameManager.current.playerKills += 1;
    }

    float DifficultyBehaviorMod(Difficulty difficulty) 
    {
        switch (difficulty) 
        {
            case Difficulty.Easy:
                return 0.10f;
            case Difficulty.Normal:
                return 0.25f;
            case Difficulty.Hard:
                return 0.50f;
            case Difficulty.Insane:
                return 0.75f;
            default:
                return 0.25f;
        }
    }

    #endregion
}

enum AIBehaviour 
{
    Patrolling,
    Searching,
    Attacking,
    Retreating,
    Dead
}