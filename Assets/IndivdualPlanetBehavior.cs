using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndivdualPlanetBehavior : MonoBehaviour
{
    private float leftStart = -10f;
    private float middleStart = 0f;
    private float rightStart = 10f;

    private float speedDown = 3f;
    private string[] planetNames = {"Pluto", "Neptune", "Uranus", "Saturn", "Jupiter", "Mars"};
    private bool planetTrigger = false;
    private PlanetBehavior mSolarSystem;


    void Start()
    {
        mSolarSystem = FindObjectOfType<PlanetBehavior>();
        if((gameObject.name == "Pluto") ||
           (gameObject.name == "Uranus"))
        {
            transform.position = new Vector3(rightStart, 25f, 0f);
        }
        else if((gameObject.name == "Neptune") ||
                (gameObject.name == "Mars"))
        {
            transform.position = new Vector3(leftStart, 25f, 0f);
        }
        else{//Saturn or Jupiter
            transform.position = new Vector3(middleStart, 25f, 0f);
        }
    }

    void Update()
    {
        if(planetTrigger){
            Vector3 pos = transform.position;
            if(pos.y > -20f){
                transform.position += (speedDown * Vector3.down) * Time.deltaTime;
            }
        }
        else{
            TriggerPlanetDescend();
        }
    }

    private void TriggerPlanetDescend(){
        if(planetNames[mSolarSystem.getPlanetCount()] == name){
            planetTrigger = true;
        }
    }
}
