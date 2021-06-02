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
    private int staticY = 7;

    private float fireRate = 1.5f;
    private float lastShot = 0.0f;
    private int numProjects = 5;
    private int[] projAngle = {-30, -15, 0, 15, 30};

    private bool visible = true;
    private bool visChange = false;
    private float lastVis = 0f;
    private float visChangeRate = 0.5f;
    private int visCount = 0;
    private int maxVis = 5;

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

        if(visChange){
            gradualVisChange();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "PlayerShip"){
            Debug.Log("Final Boss collision with player");
        }
        if(collision.gameObject.name == "mainProjectile(Clone)" && currPhase > 0){
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
            toggleVisibility();
        }
    }

    private void Phase2()
    {
        //while invisible move back and forth along y-axis(x changes)
        Vector3 endpoint1 = new Vector3(-distMax, staticY, 0);
        Vector3 endpoint2 = new Vector3(distMax, staticY, 0);
        transform.position = Vector3.Lerp (endpoint1, endpoint2, Mathf.PingPong(Time.time * speed, 1.0f));
        
        //shoot projectiles that have blast radius upon exploding
        if(Time.time > lastShot + fireRate)
        {
            fire2();
            lastShot = Time.time;
        }

        if(numCollides >= maxCollides){
            currPhase = 3;
            numCollides = 0;
            toggleVisibility(); //"breaks invisibility shield"
        }
    }

    private void Phase3()
    {        
        //while invisible move back and forth along y-axis(x changes)
        Vector3 endpoint1 = new Vector3(-distMax, staticY, 0);
        Vector3 endpoint2 = new Vector3(distMax, staticY, 0);
        transform.position = Vector3.Lerp (endpoint1, endpoint2, Mathf.PingPong(Time.time * (speed * 1.5f), 1.0f));
        
        //shoot projectiles that have blast radius upon exploding
        if(Time.time > lastShot + fireRate)
        {
            fire2();
            firePowerful2();
            lastShot = Time.time;
        }

        if(numCollides >= (maxCollides * 2)){// triggers end of final boss sequence
            mGameControl.endFinalBossSequence();
            Destroy(gameObject);
        }
    }

    private void fire5(){
        Debug.Log("FB1firreeeeeeeeeeeeeeeeeeee");
        for(int i = 0; i < numProjects; i++){
            GameObject projectile = Instantiate(Resources.Load("Prefabs/mainEnemyProjectile") as GameObject); 
            projectile.transform.localPosition = transform.localPosition;
            projectile.transform.rotation = Quaternion.Euler(0, 0, 180 + projAngle[i]);
        }
    }

    private void fire2(){
        Debug.Log("FB2firreeeeeeeeeeeeeeeeeeee");
        GameObject projectile = Instantiate(Resources.Load("Prefabs/mainEnemyProjectile") as GameObject); 
        projectile.transform.localPosition = transform.localPosition;
        projectile.transform.rotation = Quaternion.Euler(0, 0, 180 - 15);
        
        projectile = Instantiate(Resources.Load("Prefabs/mainEnemyProjectile") as GameObject); 
        projectile.transform.localPosition = transform.localPosition;
        projectile.transform.rotation = Quaternion.Euler(0, 0, 180 + 15);
    }

    private void firePowerful2(){
        Debug.Log("FB2firreeeeeeeeeeeeeeeeeeee");
        GameObject projectile = Instantiate(Resources.Load("Prefabs/mainEnemyTrackingProjectile") as GameObject); 
        projectile.transform.localPosition = transform.localPosition;
        projectile.transform.rotation = Quaternion.Euler(0, 0, 180 - 15);
        
        projectile = Instantiate(Resources.Load("Prefabs/mainEnemyTrackingProjectile") as GameObject); 
        projectile.transform.localPosition = transform.localPosition;
        projectile.transform.rotation = Quaternion.Euler(0, 0, 180 + 15);
    }

    private void toggleVisibility(){
        resetVisibility(visible); //in case, moves to next phase in the middle of vis change
        visible = !visible;
        visChange = true;
        lastVis = Time.time;
    }

    private void gradualVisChange(){
        //gradually increase/decrease visibility
        Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
        if(!visible){//changing to invisible
            tmp.a = 1f - (visCount * .25f);
        }
        else{
            tmp.a = 0f + (visCount * .25f);
        }
        gameObject.GetComponent<SpriteRenderer>().color = tmp;
        
        if(Time.time > lastVis + visChangeRate)
        {
            lastVis = Time.time;
            visCount++;
        }
        if(visCount == maxVis){
            Debug.Log("vischangeeee");
            visChange = false;
            visCount = 0;
        }
    }
    
    private void resetVisibility(bool vis){
        Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
        if(vis){
            tmp.a = 1f;
        }
        else{
            tmp.a = 0f;
        }
        gameObject.GetComponent<SpriteRenderer>().color = tmp;
    }
}
