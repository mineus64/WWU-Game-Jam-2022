// This is a script to control the visual sound object and its scale and colour. It should be attached to the Sound prefab.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    #region Variables

    [Header("Common Sound Values")]
    [SerializeField] AnimationCurve valuesOverTime;
    [SerializeField] Gradient colourRange;
    [SerializeField] float expansionRate;
    [SerializeField] float fadeRate;

    [Header("Sound Properties")]
    [SerializeField] float volume;

    [Header("Objects")]
    [SerializeField] GameObject sphere;
    [SerializeField] MeshRenderer sphereRenderer;

    #endregion
    
    #region General Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (volume <= 0) {
            Destroy(this.gameObject);
        }  

        volume -= fadeRate * Time.deltaTime; 

        transform.localScale += Vector3.one * (expansionRate * Time.deltaTime);

        float volumeFactor = volume * 0.01f;

        sphereRenderer.sharedMaterial.SetFloat("Volume", valuesOverTime.Evaluate(volumeFactor));
    }

    #endregion

    #region Specific Methods

    public void SetProperties(float volume) 
    {
        this.volume = Mathf.Clamp(volume, 0, 100);
    }

    #endregion
}
