// This script controls the camera. It should be on the main camera in the game scene.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    #region Singleton

    public static CameraController current = null;

    #endregion

    #region Variables

    float verticalRotation = 0.0f;

    [Header("Objects")]
    [SerializeField] GameObject camParent;

    [Header("Values")]
    [SerializeField] float horizontalSensitivity = 1.0f;
    [SerializeField] float verticalSensitivity = 1.0f;

    [Header("Constraints")]
    [SerializeField] float upDownLock = 15f;

    #endregion

    #region General Methods

    void Awake() 
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetParent(GameManager.current.currentClient.cameraAnchor, GameManager.current.currentClient.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseMove = new Vector2(
            Input.GetAxis("Mouse Y") * verticalSensitivity,
            Input.GetAxis("Mouse X") * horizontalSensitivity
        );

        // Horizontal rotation
        camParent.transform.Rotate(new Vector3(0.0f, mouseMove.y, 0.0f));



        /*
        camParent.transform.eulerAngles = new Vector3(
            0.0f,
            camParent.transform.eulerAngles.y,
            0.0f
        );
        */

        // Vertical Rotation
        verticalRotation += -mouseMove.x;

        this.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        if (verticalRotation < -upDownLock) verticalRotation = -upDownLock;
        if (verticalRotation > upDownLock + 10) verticalRotation = upDownLock + 10;
        // I KNOW THIS ISN'T CONSTRAINED BUT I HAVE BEEN TRYING TO FIX IT AND IT DOESN'T WORK
        // YOU FIX IT

        this.transform.eulerAngles = new Vector3(
            this.transform.eulerAngles.x,
            this.transform.eulerAngles.y,
            0.0f
        );

        

    }

    void LateUpdate() {
        
    }

    #endregion

    #region Specific Methods

    public void SetParent(GameObject cameraAnchor, GameObject cameraFollowing = null) 
    {
        this.transform.SetParent(cameraAnchor.transform);
        this.transform.position = cameraAnchor.transform.position;

        camParent = cameraFollowing;
    }

    #endregion
};