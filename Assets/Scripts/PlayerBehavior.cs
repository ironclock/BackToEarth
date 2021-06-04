using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Movement Behavior")]
    public float Speed = 10f;


    [Header("Sprite Animation")]
    public Sprite originalSprite;
    public Sprite pressedLeftSprite;
    public Sprite pressedRightSprite;

    [Header("Health System")]
    public GameObject healthBar;

    private SpriteRenderer spriteRenderer; 
    private Vector3 position;

    private GameObject mainProjectile;
    private float fireRate = 0.3f;
    private float lastShot = 0.0f;

    private int maxHealth = 8;
    private int currHealth;
    
    void Start()
    {
        currHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = originalSprite; 
        }
    }

    void Update()
    {

        /* Movement Behavior */
        spriteRenderer.sprite = originalSprite;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            position += Vector3.left * Speed * Time.deltaTime * 2;
            spriteRenderer.sprite = pressedLeftSprite;
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            position += Vector3.right * Speed * Time.deltaTime * 2;
            spriteRenderer.sprite = pressedRightSprite;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            position += Vector3.up * Speed * Time.deltaTime * 2;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            position += Vector3.down * Speed * Time.deltaTime * 2;
        }
      
        //position.x = transform.position.x + Speed * Time.deltaTime;
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
        --currHealth;
        healthBar.transform.GetChild(currHealth).gameObject.GetComponent<Renderer>().enabled = false;

        if (collision.gameObject.tag == "EnemyProjectile")
        {
            Destroy(collision.gameObject);
        }

        if (currHealth <= 0)
        {
            playerDies();
        }
    }

    private void playerDies()
    {
        Debug.Log("player dies");
        //player dies: level restarts
        SceneManager.LoadScene("SpaceShooter"); //Load scene called Game.
        //Destroy(gameObject);
    }
}
