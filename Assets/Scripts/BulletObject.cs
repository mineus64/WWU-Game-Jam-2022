using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    public float FiringSpeed;
    public float Damage;

    public void Initialize(float firingSpeed, float damage){

        FiringSpeed = firingSpeed;
        Damage = damage;

    }

    public void Start(){

        StartCoroutine(DeathTimer());

    }
    public void OnCollisionEnter(Collision col){
        if (!col.gameObject.CompareTag("Player")){
            if (col.gameObject.CompareTag("Enemy")){
                col.gameObject.GetComponent<AIController>().DeltaHealth(-Damage);
            }
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
