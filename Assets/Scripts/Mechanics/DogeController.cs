using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogeController : MonoBehaviour
{
    //combat related
    public float health;
    public float fireDelay;
    private float nextFireTime;

    
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
    Vector3 characterScale;
    float characterScaleX;

    // origin position, reset when die
    private Vector3 originPos;

    void Awake()
    {
        health = 100;

        rb2d = GetComponent<Rigidbody2D> ();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        
        characterScale = transform.localScale;
        characterScaleX = characterScale.x;
        
        dashing = false;
    }

    private void Start()
    {
        originPos = transform.position;
    }

    void Update()
    {
        tryJumping();
        adjustGravity();
        checkDash();
        tryShooting();

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
    private void orientCharacter(float direction)
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
            projectile.transform.localPosition = transform.localPosition;
            if (characterScale.x < 0) //right
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
        if (collision.gameObject.tag == "DeathZone")
        {
            Debug.Log("Die!");
            transform.position = originPos;
        }
    }
}
