using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipBehavior : MonoBehaviour
{
    private GameControl mGameControl = null;

    private float speedEnemy1 = 1f;
    private float speedEnemy2 = 2f;

    private int maxCollides = 3;
    private int numCollides = 0;

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
        }
        if(gameObject.name == "EnemyShip2(Clone)"){
            transform.position += (speedEnemy2 * Vector3.down) * Time.deltaTime;
        }
        transform.position = mGameControl.CheckEnemyOutOfView(transform.position, gameObject);
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
}
