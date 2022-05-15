// This is a script to control the visual sound object and its scale and colour. It should be attached to the Sound prefab.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    #region Variables

    [Header("Common Sound Values")]
    [SerializeField] AnimationCurve opacityOverTime;
    [SerializeField] AnimationCurve colourOverTime;
    [SerializeField] Gradient colourRange;
    [SerializeField] float expansionRate;

    [Header("Sound Properties")]
    [SerializeField] float volume;

    [Header("Objects")]
    [SerializeField] GameObject sphere;
    [SerializeField] MeshRenderer sphererenderer;

    #endregion
    
    #region General Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        volume -= Time.deltaTime; 

        transform.localScale += Vector3.one * (expansionRate * Time.deltaTime);

        float volumeFactor = volume * 0.01f;

        if (volume <= 0) {
            Destroy(this.gameObject);
        }  
    }

    #endregion

    #region Specific Methods



    #endregion
}
