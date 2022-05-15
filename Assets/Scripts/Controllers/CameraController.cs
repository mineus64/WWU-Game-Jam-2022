// This script controls the camera. It should be on the main camera in the game scene.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Singleton

    public static CameraController current = null;

    #endregion

    #region Variables



    #endregion

    #region General Methods

    void Awake() 
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetParent(GameManager.current.currentClient.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Specific Methods

    public void SetParent(GameObject parent) 
    {
        this.transform.SetParent(parent.transform);
        this.transform.position = parent.transform.position;
    }

    #endregion
}
