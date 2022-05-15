using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    #region Variables

    [Header("Timer")]
    [SerializeField] float refreshTimer;        // The duration remaining until this pickup refreshes
    [SerializeField] float refreshDuration;     // The length of time between item refreshes

    [Header("Loot Information")]
    [SerializeField] int[] ammoDrops = 
    {
        16,     // Pistol
        5,      // Anti-Material Rifle
        30,     // Assault Rifle
        1,      // Big Bang Grenade
        1,      // Decoy Grenade
        1,      // Frag Grenade
        8,      // Pistol (Suppressed)
        1,      // Rocket Launcher
        6,      // Shotgun
        30      // SMG
    };
    [SerializeField] int currentLoot;

    [Header("Collider")]
    [SerializeField] BoxCollider colliderObj;

    [Header("Display Info")]
    [SerializeField] float spinSpeed;
    [SerializeField] GameObject lootPreviewAnchor;
    [SerializeField] GameObject lootPreview;

    #endregion
    
    #region General Methods

    void Awake() 
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Decrement the refresh timer
        refreshTimer = Mathf.Max(refreshTimer - Time.deltaTime, 0.0f);

        // Check if we need to refresh the item and, if so, refresh it
        if (refreshTimer <= 0 && currentLoot == -1) {
            RefreshLoot();
        }

        // Spin the item if valid
        if (currentLoot != -1) {
            lootPreviewAnchor.transform.Rotate(Vector3.up * spinSpeed);
        }

    }

    #endregion

    #region Specific Methods

    void OnTriggerEnter(Collider collision) 
    {
        if (currentLoot != -1) {
            // This assumes that only Players or AI will be interacting, this needs to be fixed for the general case
            if (collision.gameObject.GetComponent<PlayerController>() != null) {
                PlayerController controller = collision.gameObject.GetComponent<PlayerController>();

                controller.AddWeapon(currentLoot, ammoDrops[currentLoot]);
            }
            else {

            }

            currentLoot = -1;

            refreshTimer = refreshDuration;

            Destroy(lootPreview);
        }
    }

    void RefreshLoot() 
    {
        int loot = (int)Random.Range(0, ammoDrops.Length - 1);

        currentLoot = loot;

        lootPreview = Instantiate(GameManager.current.weapons[loot].weaponObject, lootPreviewAnchor.transform);
    }

    #endregion
}
