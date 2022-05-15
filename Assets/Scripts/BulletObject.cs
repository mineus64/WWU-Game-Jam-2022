using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    public float FiringSpeed;

    public BulletObject(float firingSpeed){

        FiringSpeed = firingSpeed;

    }

    public void OnTriggerEnter(Collider other){

        Destroy(this);

    }

}
