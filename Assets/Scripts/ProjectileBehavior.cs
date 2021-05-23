using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float kProjSpeed;
    public const int kLifetime = 300; // alive for this many cycles
    
    public int mLifeCount = 0;

    void Start()
    {
        mLifeCount = kLifetime;
        if(gameObject.name == "mainProjectile(Clone)"){
            kProjSpeed = 20f;
        }
        if(gameObject.name == "mainEnemyProjectile(Clone)"){
            kProjSpeed = 10f;
        }
        if(gameObject.name == "mainEnemyTrackingProjectile(Clone)"){
            kProjSpeed = 5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.name == "mainEnemyTrackingProjectile(Clone)"){
            Vector3 target = GameObject.Find("PlayerShip").transform.position;
            transform.position = Vector3.MoveTowards(transform.position, target, kProjSpeed * Time.deltaTime);
        }
        else{
            transform.position += ((kProjSpeed * Time.smoothDeltaTime) * transform.up);
        }
        mLifeCount--;
        if(mLifeCount <= 0)
        {
            Destroy(gameObject); // ends instance of self
        }
    }
}
