using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBulletBehaviour : MonoBehaviour
{
    public const float kMainProjSpeed = 10f;
    public const int kLifetime = 1000; // alive for this many cycles

    public int mLifeCount = 0;

    void Start()
    {
        mLifeCount = kLifetime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += ((kMainProjSpeed * Time.smoothDeltaTime) * transform.up);
        mLifeCount--;
        if (mLifeCount <= 0)
        {
            Destroy(gameObject); // ends instance of self
        }
    }
}
