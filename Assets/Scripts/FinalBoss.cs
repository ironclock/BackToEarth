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

    void Start()
    {
        mGameControl = FindObjectOfType<GameControl>();
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
        if(pos.y <= 7 ){
            currPhase = 1;
        }
    }

    private void Phase1()
    {
        //move back and forth along y-axis(x changes)
        //shoot 5 blue projectiles
        if(numCollides >= maxCollides){//temp, triggers end of final boss sequence
            mGameControl.endFinalBossSequence();
            Destroy(gameObject);
        }
    }

    private void Phase2()
    {
        //move along y-axis(x changes) randomly every 2 seconds
        //shoot projectiles that have blast radius upon exploding
    }

    private void Phase3()
    {
        //move randomly on y-axis when firing, invisible but appears briefly when firing
        //shoot projectiles that are invisible until right before they explode, small blast radius, hearing firing alerts player too
        // if(numCollides >= maxCollides){
        //     mGameControl.endFinalBossSequence();
        // }
    }
}
