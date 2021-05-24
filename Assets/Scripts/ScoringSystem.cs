using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour
{
    public Text mScore = null;

    private int mEnemy1Value = 3;
    private int mEnemy2Value = 5;
    private int mFinalBossValue = 30;
    private int scoreValue = 0;
    private int mFinalBossThreshold = 70;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddEnemy1ToScore()
    {
        scoreValue += mEnemy1Value;
        mScore.text = "SCORE: " + scoreValue;
    }

    public void AddEnemy2ToScore()
    {
        scoreValue += mEnemy2Value;
        mScore.text = "SCORE: " + scoreValue;
    }

    public void AddFinalBossToScore()
    {
        scoreValue += mFinalBossValue;
        mScore.text = "SCORE: " + scoreValue;
    }

    public int getScore()
    {
        return scoreValue;
    }
    public int getFinalBossThreshold()
    {
        return mFinalBossThreshold;
    }
}
