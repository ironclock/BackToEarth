using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryBehaviour : MonoBehaviour
{
    public float shootingAngle;
    
    private GameObject mainProjectile;
    public float fireRate = 2.0f;
    private float lastShot = 0.0f;

    void Update()
    {
        if (Time.time > fireRate + lastShot)
        {
            int randBulletCounter = Random.Range(1, 4);
            for (int i = 0; i < randBulletCounter; i++)
            {
                mainProjectile = Instantiate(Resources.Load("Prefabs/Egg") as GameObject);
                Physics2D.IgnoreCollision(mainProjectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                mainProjectile.transform.localPosition = transform.localPosition;
                mainProjectile.transform.rotation = Quaternion.Euler(0, -180, shootingAngle + i * 4);
                lastShot = Time.time;
            }
        }
    }
}
