using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class GameControl : MonoBehaviour
{
    private const int kMaxEnemy1 = 4;
    private const int kMaxEnemy2 = 2;

    private int mTotalEnemy1 = 0;
    private int mTotalEnemy2 = 0;
    private Vector2 mSpawnRegionMin, mSpawnRegionMax;
    private int mEnemyDestroyed = 0;
    private float xOFfset = 3f;
    private float yOFfset = 3f;
    private bool halt = false;
    private bool finalBossGenerated = false;
    private bool earthGenerated = false;
    private bool earthTime = false;
    private float speedDown = 2f;
    
    private float genRate = 1f;
    private float lastGen = 0.0f;

    private ScoringSystem mScoringSystem;
    private PlanetBehavior mSolarSystem;

    public Camera cam;
    public Bounds b;
    void Start()
    {
        mScoringSystem = FindObjectOfType<ScoringSystem>();
        mSolarSystem = FindObjectOfType<PlanetBehavior>();
    }

    void Update()
    {
        
        if (Input.GetKey(KeyCode.Q))
        {
            Application.Quit();
        }
        if(Time.time > lastGen + genRate)
        {
            if(mTotalEnemy1 < kMaxEnemy1){
                GenerateEnemy1();
            }
            if(mTotalEnemy2 < kMaxEnemy2){
                GenerateEnemy2();
            }
            lastGen = Time.time;
        }

        if((mScoringSystem.getScore() >= mScoringSystem.getFinalBossThreshold()) && !finalBossGenerated){
            finalBossGenerated = true;
            initiateFinalBossSequence();
        }

        if(earthTime){
            EarthAppears();
        }
        
        CheckTime4NextPlanet();
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
        float y = Random.Range(mSpawnRegionMax.y + yOFfset, 1.5f * mSpawnRegionMax.y); //spawns in this range above screen
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
    public Vector3 CheckEnemyOutOfView(Vector3 pos, GameObject g, ref bool stopFiring) { 
        if(pos.y < (mSpawnRegionMin.y - 2f)){ // to make sure it's off screen
            if(halt){//halted for final boss
                //destroy enemy
                Destroy(g);
            }
            else{//reposition 
                pos.x = Random.Range(mSpawnRegionMin.x + xOFfset, mSpawnRegionMax.x - xOFfset);
                pos.y = Random.Range(mSpawnRegionMax.y + yOFfset, 1.5f * mSpawnRegionMax.y); //spawns in this range above screen
            }
        }
        Vector3 playerPos = GameObject.Find("PlayerShip").transform.position;
        if(pos.y < playerPos.y){ // stop firing if past player
            stopFiring = true;
        }
        else{// keep firing if repositioned
            stopFiring = false;
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
            SceneManager.LoadScene("Scenes/Platformer"); 
        }
    }

    private void CheckTime4NextPlanet(){
        float score = mScoringSystem.getScore();
        float planCt = mSolarSystem.getPlanetCount() + 1;
        int numPlans = 8;
        float end = mScoringSystem.getFinalBossThreshold();
        if(score >= (planCt / numPlans * end) ){
            Debug.Log(planCt / numPlans * end);
            mSolarSystem.NextPlanet();
        }
    }
}
