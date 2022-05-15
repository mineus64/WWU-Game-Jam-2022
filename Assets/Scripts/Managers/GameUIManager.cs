// This script manages the ingame UI. It should be attached to the game canvas in the game scene.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    #region Singleton

    public static GameUIManager current = null;

    #endregion

    #region Variables

    [Header("Ammo Counter")]
    [SerializeField] Image magAmmoCount;
    [SerializeField] Image resAmmoCount;
    [SerializeField] Image weaponIcon;

    #endregion

    #region General Methods

    void Awake() 
    {
        current = this;
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

    public void UpdateMagAmmoCount(int magAmmoCurrent, int magAmmoMax) 
    {
        magAmmoCount.fillAmount = (magAmmoCurrent / magAmmoMax) / 0.5f;
    }

    public void UpdateResAmmoCount(int resAmmoCurrent, int resAmmoMax) 
    {
        resAmmoCount.fillAmount = (resAmmoCurrent / resAmmoMax) / 0.5f;
    }

    public void UpdateWeaponIcon(Sprite weaponIcon) 
    {
        this.weaponIcon.sprite = weaponIcon;
    }

    #endregion
}
