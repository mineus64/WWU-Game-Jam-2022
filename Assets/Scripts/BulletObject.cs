using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    public float FiringSpeed;

    public BulletObject(float firingSpeed){

        FiringSpeed = firingSpeed;

    }

    public void Start(){

        StartCoroutine(DeathTimer());

    }
    public void OnCollisionEnter(Collision col){
        if (!col.gameObject.CompareTag("Player")){
        Destroy(this.gameObject);
        }

    }

    public IEnumerator DeathTimer(){
        float time = 2f;
        float totalTime = 0f;
        while(totalTime < time){
            totalTime += Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
    }

}
