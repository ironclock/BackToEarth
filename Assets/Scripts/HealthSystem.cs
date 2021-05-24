using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Slider healthSlider;
    public Image Fill;

    private const int totalHealth = 100;
    private const int damageFromEnemyCollision = 15;
    private const int damageFromProjectile = 5;
    private const int damageFromTrackingProjectile = 10;
    private int currHealth;


    void Start()
    {
        currHealth = totalHealth;
        healthSlider.maxValue = totalHealth;
        // Fill.color = Color.green;
    }

    void Update()
    {
        healthSlider.value = currHealth;
        if(currHealth < 50){
            Fill.color = Color.yellow;
        }
        if(currHealth < 25){
            Fill.color = Color.red;
        }
    }

    public void AddDamageFromProjectile(string projType)
    {
        if(projType == "mainEnemyProjectile(Clone)"){
            currHealth -= damageFromProjectile;
        }
        if(projType == "mainEnemyTrackingProjectile(Clone)"){
            currHealth -= damageFromTrackingProjectile;
        }
    }

    public void AddDamageFromEnemy()
    {
        currHealth -=damageFromEnemyCollision;
    }

    public int getHealth()
    {
        return currHealth;
    }
}
