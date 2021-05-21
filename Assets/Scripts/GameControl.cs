using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    private const int kMaxEnemy1 = 5;
    private const int kMaxEnemy2 = 2;

    private int mTotalEnemy1 = 0;
    private int mTotalEnemy2 = 0;
    private Vector2 mSpawnRegionMin, mSpawnRegionMax;
    private int mEnemyDestroyed = 0;
    private float xOFfset = 3f;
    private bool halt = false;
    private bool finalBossGenerated = false;
    private bool earthGenerated = false;
    private bool earthTime = false;
    private float speedDown = 1f;

    private ScoringSystem mScoringSystem;

    public Camera cam;
    public Bounds b;
    void Start()
    {
        mScoringSystem = FindObjectOfType<ScoringSystem>();
    }

    void Update()
    {
        
        if (Input.GetKey(KeyCode.Q))
        {
            Application.Quit();
        }

        if(mTotalEnemy1 < kMaxEnemy1){
            GenerateEnemy1();
        }
        if(mTotalEnemy2 < kMaxEnemy2){
            GenerateEnemy2();
        }

        if((mScoringSystem.getScore() >= mScoringSystem.getFinalBossThreshold()) && !finalBossGenerated){
            finalBossGenerated = true;
            initiateFinalBossSequence();
        }

        if(earthTime){
            EarthAppears();
        }
    }
    public void GenerateEnemy1(){
        GameObject p = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/EnemyShip") as GameObject);
        p.transform.position = SetEnemyPosition(transform.position);
        mTotalEnemy1++;
    }
    public void GenerateEnemy2(){
        GameObject p = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/EnemyShip2") as GameObject);
        p.transform.position = SetEnemyPosition(transform.position);
        mTotalEnemy2++;
    }
    private Vector3 SetEnemyPosition(Vector3 pos){
        cam = Camera.main;
        b.size = new Vector3(2 * cam.orthographicSize * cam.aspect, 2 * cam.orthographicSize, 1f);
        mSpawnRegionMin = b.min;
        mSpawnRegionMax = b.max;

        float x = Random.Range(mSpawnRegionMin.x + xOFfset, mSpawnRegionMax.x - xOFfset);
        float y = Random.Range(mSpawnRegionMax.y, 2 * mSpawnRegionMax.y); //spawns in this range above screen
        pos = new Vector3(x, y, 0f);
        return pos;
    }
    
    public void OneEnemyDestroyed(string enemyType) { 
        if(enemyType == "EnemyShip(Clone)"){
            mTotalEnemy1--;
            mScoringSystem.AddEnemy1ToScore();
            GenerateEnemy1();
        }
        if(enemyType == "EnemyShip2(Clone)"){
            mTotalEnemy2--;
            mScoringSystem.AddEnemy2ToScore();
            GenerateEnemy2();
        }
        mEnemyDestroyed++;
    }
    public Vector3 CheckEnemyOutOfView(Vector3 pos, GameObject g) { 
        if(pos.y < (mSpawnRegionMin.y - 2f)){ // to make sure it's off screen
            if(halt){//halted for final boss
                //destroy enemy
                Destroy(g);
            }
            else{
                pos.x = Random.Range(mSpawnRegionMin.x + xOFfset, mSpawnRegionMax.x - xOFfset);
                pos.y = Random.Range(mSpawnRegionMax.y, 2 * mSpawnRegionMax.y); //spawns in this range above screen
            }
        }
        return pos;
    }

    private void haltEnemyGeneration()
    {
        halt = true;
    }

    private void initiateFinalBossSequence()
    {
        //stop generation new type 1 and 2 enemies
        haltEnemyGeneration();
        //make screen stop scrolling
        //generate final boss prefab
        GameObject p = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/FinalBoss") as GameObject);
        float y = mSpawnRegionMax.y * 2f; //spawns in this range above screen
        p.transform.position = new Vector3(0f, y, 0f);
        //final boss sequence starts
    }

    public void endFinalBossSequence()
    {
        //start screen scrolling again
        //add final boss to score
        mScoringSystem.AddFinalBossToScore();
        //make earth pop up
        GameObject p = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Earth") as GameObject);
        float y = mSpawnRegionMax.y * 2.8f; //spawns in this range above screen
        p.transform.position = new Vector3(0f, y, 0f);
        earthTime = true;
    }

    private void EarthAppears()
    {
        GameObject p = GameObject.Find("Earth(Clone)");
        Vector3 pos = p.transform.position;
        if(pos.y > 0f){
            p.transform.position += (speedDown * Vector3.down) * Time.deltaTime;
        }
        if(pos.y <= 0f){
            //screen stops scrolling
            //player shrinks down to earth
        }
    }

}
