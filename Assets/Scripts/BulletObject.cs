using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    public float FiringSpeed;
    public float Damage;

    public bool IsPlayerBullet;

    public void Initialize(float firingSpeed, float damage){

        FiringSpeed = firingSpeed;
        Damage = damage;

    }

    public void Start(){
        
        if(this.transform.parent.gameObject.CompareTag("Player")){
            IsPlayerBullet = true;
        } else if (this.transform.parent.gameObject.CompareTag("Enemy")){
            IsPlayerBullet = false;
        }
        this.transform.SetParent(null);
        StartCoroutine(DeathTimer());

    }
    public void OnCollisionEnter(Collision col){
        if(IsPlayerBullet){
            if (col.gameObject.CompareTag("Enemy")){
                col.gameObject.GetComponent<AIController>().DeltaHealth(-Damage);
                Destroy(this.gameObject);
            } else if (col.gameObject.CompareTag("Finish")){
                Destroy(this.gameObject);
            }
        } else if(!IsPlayerBullet){
            if (col.gameObject.CompareTag("Player")){
                col.gameObject.GetComponent<PlayerController>().DeltaHealth(-Damage);
            } else if (col.gameObject.CompareTag("Finish")){
                Destroy(this.gameObject);
            }
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
