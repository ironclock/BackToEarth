using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBehavior : MonoBehaviour
{
    private int planetCount = 0;
    private float speedDown = 3f;
    private string[] planetz = {"Pluto", "Neptune", "Uranus", "Saturn", "Jupiter", "Mars"};
    public GameObject[] planets;

    private bool asteroidBelt = false;
    private bool planetAppearing = false;
    public GameObject p;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void NextPlanet(){
        if(asteroidBelt){
            //asteroidBelt stuff
        }
        else{
            //planetCount++;
            if(planetCount == 6)
            {
                asteroidBelt = true;
            }
        }
    }

    public int getPlanetCount(){
        return planetCount;
    }
    public void incrementPlanetCount(){
        planetCount++;
    }
}
