using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryBehaviour : MonoBehaviour
{
    private GameObject mainProjectile;
    private float fireRate = 2.0f;
    private float lastShot = 0.0f;

    void Update()
    {
        if (Time.time > fireRate + lastShot)
        {
            int randBulletCounter = Random.Range(1, 4);
            for (int i = 0; i < randBulletCounter; i++)
            {
                mainProjectile = Instantiate(Resources.Load("Prefabs/Egg") as GameObject);
                mainProjectile.transform.localPosition = transform.localPosition;
                mainProjectile.transform.rotation = Quaternion.Euler(0, 0, 90 - i * 4);
                lastShot = Time.time;
            }
        }
    }
}
