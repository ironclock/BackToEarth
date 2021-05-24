using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogeController : MonoBehaviour
{
    //combat related
    public float health;
    public float fireDelay;
    private float nextFireTime;
    public Transform firePoint;

    
    //speed/movement variables
    private Rigidbody2D rb2d;
    public float currentSpeed;
    public float maxSpeed;
    public float acceleration;
    //dash
    public float dashPower;
    public float dashCooldown;
    public float dashTime;
    public float dashKeyPressInterval;
    private float lastDash;
    private float firstDashKeyPressed;
    private bool leftPressedOnce;
    private bool rightPressedOnce;
    private bool dashing;

    //jump variables
    public int extraJumps;
    private int jumpCount;
    public float jumpPower;
    [SerializeField] private LayerMask platformsLayerMask;
    private BoxCollider2D boxCollider2D;
    public bool grounded = true;

    //character visual variables
    private Vector3 characterScale;
    float characterScaleX;

    // origin position, reset when die
    public Vector3 respawnPos;

    //starting "animation"
    public GameObject spaceship;
    public bool playerMovable;
    public Animator animator;

    void Awake()
    {
        health = 100;
        playerMovable = false;

        rb2d = GetComponent<Rigidbody2D> ();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        
        characterScale = transform.localScale;
        characterScaleX = characterScale.x;
        
        dashing = false;
    }

    private void Start()
    {
    }

    void Update()
    {
        if (playerMovable)
        {
            if (IsGrounded())
            {
                animator.SetFloat("speed", Mathf.Abs(rb2d.velocity.x));
            }
            if (rb2d.velocity.y > 0.01)
            {
                animator.SetBool("going_up", true);
            }
            else
            {
                animator.SetBool("going_up", false);
            }

            tryJumping();
            adjustGravity();
            checkDash();
            //tryShooting();

            if (dashing == false)
            {
                float directionX = Input.GetAxisRaw("Horizontal");
                if (directionX == 0) //not moving
                {
                    currentSpeed = 0;
                }
                else
                {
                    if (Mathf.Abs(currentSpeed) < maxSpeed) //increment speed
                    {
                        currentSpeed += acceleration * directionX;
                    }
                    else //limit to max speed
                    {
                        currentSpeed = maxSpeed * directionX;
                    }
                    rb2d.velocity = new Vector2(currentSpeed, rb2d.velocity.y); //add velocity
                }
                orientCharacter(directionX);
            }
        }
        else
        {
            transform.position = spaceship.transform.position;
        }
    }

    private bool IsGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        return raycastHit2D.collider != null;
    }
    private void adjustGravity()
    {
        rb2d.gravityScale = 1.25f;
        if (rb2d.velocity.y < 4)
        {
            rb2d.gravityScale = 22f;
        }
    }
    public void orientCharacter(float direction)
    {
        if (direction < 0) {
            characterScale.x = characterScaleX;
        }
        if (direction > 0)
        {
            characterScale.x = -characterScaleX;
        }
        transform.localScale = characterScale;
    }
    private void tryJumping()
    {
        if(Input.GetKeyDown(KeyCode.Space) && jumpCount < extraJumps) {
            rb2d.velocity = Vector2.up * jumpPower;
            jumpCount++;
        }
        if (IsGrounded())
        {
            jumpCount = 0;
        }
    }
    private void checkDash()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (canDashLeft())
            {
                dash(-1);
                dashing = true;
                lastDash = Time.time;
                leftPressedOnce = false;
                rightPressedOnce = false;
            }
            else
            {
                leftPressedOnce = true;
                rightPressedOnce = false;
                firstDashKeyPressed = Time.time;
            }
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (canDashRight())
            {
                dash(1);
                dashing = true;
                lastDash = Time.time;
                leftPressedOnce = false;
                rightPressedOnce = false;
            }
            else
            {
                leftPressedOnce = false;
                rightPressedOnce = true;
                firstDashKeyPressed = Time.time;
            }
        }
        if (Time.time - lastDash > dashTime)
        {
            dashing = false;
        }
    }
    private bool canDashLeft()
    {
        float timeSinceFirstKey = Time.time - firstDashKeyPressed;
        return (leftPressedOnce && timeSinceFirstKey < dashKeyPressInterval && Time.time - lastDash > dashCooldown);
    }
    private bool canDashRight()
    {
        float timeSinceFirstKey = Time.time - firstDashKeyPressed;
        return (rightPressedOnce && timeSinceFirstKey < dashKeyPressInterval && Time.time - lastDash > dashCooldown);
    }
    private void dash(int directionX)
    {
        rb2d.velocity = new Vector2(maxSpeed * dashPower * directionX, rb2d.velocity.y);
    }

    private void tryShooting()
    {
        if (Time.time > nextFireTime && Input.GetKey(KeyCode.Mouse0))
        {
            GameObject projectile = Instantiate(Resources.Load("Prefabs/playerProjectile") as GameObject);
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            projectile.transform.position = firePoint.position;
            if (characterScale.x > 0) //right
            {
                projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeed * 2, 0);
            }
            else //left
            {
                projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeed * -2, 0);
            }
            nextFireTime = Time.time + fireDelay;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DeathZone" || collision.gameObject.tag == "PBullet")
        {
            Debug.Log("Die!");
            transform.position = respawnPos;
        }
    }
}
