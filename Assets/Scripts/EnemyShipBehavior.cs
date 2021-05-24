using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipBehavior : MonoBehaviour
{
    private GameControl mGameControl = null;

    private float speedEnemy1 = 2f;
    private float speedEnemy2 = 3f;

    private int maxCollides = 2;
    private int numCollides = 0;
    
    private float fireRate = 3f;
    private float lastShot = 0.0f;
    public bool stopFiring = false;

    void Start()
    {
        mGameControl = FindObjectOfType<GameControl>();
        transform.Rotate(0f, 0f, 180f);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.name == "EnemyShip(Clone)"){
            transform.position += (speedEnemy1 * Vector3.down) * Time.deltaTime;
            if((Time.time > lastShot + fireRate) && !stopFiring)
            {
                enemy1Fire();
                lastShot = Time.time;
            }
        }
        if(gameObject.name == "EnemyShip2(Clone)"){
            transform.position += (speedEnemy2 * Vector3.down) * Time.deltaTime;
            if((Time.time > lastShot + fireRate) && !stopFiring)
            {
                enemy2Fire();
                lastShot = Time.time;
            }
        }
        transform.position = mGameControl.CheckEnemyOutOfView(transform.position, gameObject, ref stopFiring);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "PlayerShip"){
            Debug.Log("Enemy collision with player");
        }
        if(collision.gameObject.name == "mainProjectile(Clone)"){
            Debug.Log("Enemy collision with projectile");
            numCollides++;
            Destroy(collision.gameObject);
            if(numCollides == maxCollides){
                mGameControl.OneEnemyDestroyed(gameObject.name);
                Destroy(gameObject);
            }
        }
    }

    private void enemy1Fire(){
        GameObject projectile = Instantiate(Resources.Load("Prefabs/mainEnemyProjectile") as GameObject); 
        projectile.transform.localPosition = transform.localPosition;
        projectile.transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    private void enemy2Fire(){
        GameObject projectile = Instantiate(Resources.Load("Prefabs/mainEnemyTrackingProjectile") as GameObject); 
        projectile.transform.localPosition = transform.localPosition;
        projectile.transform.rotation = Quaternion.Euler(0, 0, -90);
    }
}
