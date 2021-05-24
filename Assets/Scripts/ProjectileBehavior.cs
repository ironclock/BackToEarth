using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float kProjSpeed;
    public const int kLifetime = 600; // alive for this many cycles
    
    public int mLifeCount = 0;

    void Start()
    {
        transform.Rotate(180.0f, 180.0f, 180.0f);

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
        
            Vector3 lookDirection = target - new Vector3(transform.position.x, transform.position.y);
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            if (mLifeCount > 0.7 * kLifetime || angle < 30) //turns during the start of its life, or if the ship is within a cone of the missile (angle < coneAngle)
            {
                Quaternion qTo = Quaternion.Euler(new Vector3(0, 0, angle));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, 45f * Time.deltaTime);
            }
            transform.position += transform.up * (kProjSpeed * Time.smoothDeltaTime);
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
