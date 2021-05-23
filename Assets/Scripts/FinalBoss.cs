using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    private GameControl mGameControl = null;
    private int currPhase = 0;

    private int maxCollides = 3;
    private int numCollides = 0;
    private float speedDown = 1f;

    private float speed = 0.5f;
    private int distMax = 13;
    private bool moveRight = true;
    private int staticY = 7;

    private float fireRate = 1.5f;
    private float lastShot = 0.0f;
    private int numProjects = 5;
    private int[] projAngle = {-30, -15, 0, 15, 30};

    void Start()
    {
        mGameControl = FindObjectOfType<GameControl>();
        //start with collider off
    }

    void Update()
    {
        if(currPhase == 0){
            Phase0();
        }
        if(currPhase == 1){
            Phase1();
        }
        if(currPhase == 2){
            Phase2();
        }
        if(currPhase == 3){
            Phase3();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "PlayerShip"){
            Debug.Log("Final Boss collision with player");
        }
        if(collision.gameObject.name == "mainProjectile(Clone)"){
            Debug.Log("Final Boss collision with projectile");
            numCollides++;
            Destroy(collision.gameObject);
        }
    }

    private void Phase0(){
        //move down slowly into screen
        transform.position += (speedDown * Vector3.down) * Time.deltaTime;
        Vector3 pos = transform.position;
        if(pos.y <= staticY ){
            currPhase = 1;
            //activate collider once in screen
        }
    }

    private void Phase1()
    {
        //move back and forth along y-axis(x changes)
        Vector3 endpoint1 = new Vector3(-distMax, staticY, 0);
        Vector3 endpoint2 = new Vector3(distMax, staticY, 0);
        transform.position = Vector3.Lerp (endpoint1, endpoint2, Mathf.PingPong(Time.time * speed, 1.0f));

        //shoot 5 projectiles
        if(Time.time > lastShot + fireRate)
        {
            fire5();
            lastShot = Time.time;
        }

        if(numCollides >= maxCollides){
            currPhase = 2;
            numCollides = 0;
        }
    }

    private void Phase2()
    {
        //move along y-axis(x changes) randomly every 2 seconds
        //shoot projectiles that have blast radius upon exploding
        if(numCollides >= maxCollides){
            currPhase = 3;
            numCollides = 0;
        }
    }

    private void Phase3()
    {
        //move randomly on y-axis when firing, invisible but appears briefly when firing
        //shoot projectiles that are invisible until right before they explode, small blast radius, hearing firing alerts player too
        if(numCollides >= maxCollides){//temp, triggers end of final boss sequence
            mGameControl.endFinalBossSequence();
            Destroy(gameObject);
        }
    }

    private void fire5(){
        Debug.Log("firreeeeeeeeeeeeeeeeeeee");
        for(int i = 0; i < 5; i++){
            GameObject projectile = Instantiate(Resources.Load("Prefabs/mainEnemyProjectile") as GameObject); 
            projectile.transform.localPosition = transform.localPosition;
            projectile.transform.rotation = Quaternion.Euler(0, 0, 180 + projAngle[i]);
        }
    }
}
