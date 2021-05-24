using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Movement Behavior")]
    public float Speed = 0f;
    public float MaxSpeed = 7f;
    public float Acceleration = 50f;
    public float Deceleration = 50f;
    public float VerticalSpeed = 3f;


    [Header("Sprite Animation")]
    public Sprite originalSprite;
    public Sprite pressedLeftSprite;
    public Sprite pressedRightSprite;


    private SpriteRenderer spriteRenderer; 
    private Vector3 position;

    private GameObject mainProjectile;
    private float fireRate = 0.3f;
    private float lastShot = 0.0f;

    private HealthSystem mHealthSystem;
    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mHealthSystem = FindObjectOfType<HealthSystem>();
        if (spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = originalSprite; 
        }
    }

    void Update()
    {

        /* Movement Behavior */
        spriteRenderer.sprite = originalSprite;
        //myImageComponent.sprite  = originalSprite;
        if ((Input.GetKey(KeyCode.A)) && (Speed < MaxSpeed))
        {
            Speed = Speed - Acceleration * Time.deltaTime;  
            spriteRenderer.sprite = pressedLeftSprite;
        }
        else if ((Input.GetKey(KeyCode.D)) && (Speed > -MaxSpeed)) 
        {
            Speed = Speed + Acceleration * Time.deltaTime;
            spriteRenderer.sprite = pressedRightSprite;
        }
        else 
        {
            if (Speed > Deceleration * Time.deltaTime) 
            {
                Speed = Speed - Deceleration * Time.deltaTime;
            }
            else if (Speed < -Deceleration * Time.deltaTime) 
            {
                Speed = Speed + Deceleration * Time.deltaTime;
            }
            else
            {
                Speed = 0;
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            position += Vector3.up * VerticalSpeed * Time.deltaTime * 2;
        }

        if (Input.GetKey(KeyCode.S))
        {
            position += Vector3.down * VerticalSpeed * Time.deltaTime * 2;
        }
      
        position.x = transform.position.x + Speed * Time.deltaTime;
        transform.position = position;


        /* Main Projectile Behavior */
        if (Input.GetKey(KeyCode.Space))
        {
            //Debug.Log("Space key pressed");
            // Prefab MUST BE locaed in Resources/Prefab folder!
            if(Time.time > fireRate + lastShot)
            {
                mainProjectile = Instantiate(Resources.Load("Prefabs/mainProjectile") as GameObject); 
                mainProjectile.transform.localPosition = transform.localPosition;
                mainProjectile.transform.rotation = transform.rotation;
                lastShot = Time.time;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy"){
            Debug.Log("Enemy collision with player");
            mHealthSystem.AddDamageFromEnemy();
            //possible VFX:
            //add a bounce from bumping into enemy
            //flash red from collision
            //once player is below certain health, show dents
        }
        if(collision.gameObject.tag == "EnemyProjectile"){
            Debug.Log("Enemy collision with projectile");
            mHealthSystem.AddDamageFromProjectile(collision.gameObject.name);
            Destroy(collision.gameObject);
        }
        if(mHealthSystem.getHealth() <= 0){
            playerDies();
        }
    }

    private void playerDies()
    {
        Debug.Log("player dies");
        //player dies: level restarts
        //Destroy(gameObject);
    }
}
