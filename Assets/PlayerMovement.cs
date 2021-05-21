using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float currentSpeed;
    public float maxSpeed;
    public float acceleration;

    public int extraJumps;
    private int jumpCount;
    public float jumpPower;

    [SerializeField] private LayerMask platformsLayerMask;
    private BoxCollider2D boxCollider2D;
    public bool grounded = true;

    void Start()
    {
        rb2d = rb2d = GetComponent<Rigidbody2D> ();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (IsGrounded())
        {
            jumpCount = 0;
        }
        if(Input.GetKeyDown(KeyCode.Space) && jumpCount < extraJumps) {
            rb2d.velocity = Vector2.up * jumpPower;
            jumpCount++;
        }
        
        rb2d.gravityScale = 1.25f;
        if (rb2d.velocity.y < 0)
        {
            rb2d.gravityScale = 6f;
        }

        float x = Input.GetAxisRaw("Horizontal");
        if (x == 0)
        {
            currentSpeed = 0;
        }
        else
        {
            if (Mathf.Abs(currentSpeed) < maxSpeed)
            {
                currentSpeed += acceleration * x;
            }
            rb2d.velocity = new Vector2(currentSpeed, rb2d.velocity.y);
        }
    }

    private bool IsGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        return raycastHit2D.collider != null;
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     [Header("Horizontal Movement")]
//     public float moveSpeed = 10f;
//     public float maxSpeed = 7f;

//     [Header("Vertical Movement")]
//     public float jumpForce = 15f;
//     public float jumpDelay = 0.25f;
//     public float runningJumpForce = 5f;

//     [Header("Misc")]
//     public LayerMask groundLayer;
//     public float groundLength = 0.27f;
//     public float linearDrag = 4f;
//     public float gravity = 1f;
//     public float fallMultiplier = 5f;
//     public Vector3 colliderOffset;

//     private bool onGround = false;
//     private Rigidbody2D rb;
//     private Vector2 direction;
//     private float jumpTimer;

//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || 
//                    Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

//         if (Input.GetButtonDown("Jump"))
//         {
//             jumpTimer = Time.time + jumpDelay;
//         }

//         direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
//     }

//     private void FixedUpdate()
//     {
//         Move(direction.x);

//         if(jumpTimer > Time.time && onGround)
//         {
//             Jump();
//         }

//         ModifyPhysics();
//     }

//     private void Move(float horizontal)
//     {
//         rb.AddForce(Vector2.right * horizontal * moveSpeed);
//         if (Mathf.Abs(rb.velocity.x) >= maxSpeed)
//         {
//             rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
//             if (onGround)
//             {
//                 gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
//             }
//             else
//             {
//                 gameObject.GetComponent<SpriteRenderer>().color = Color.red;
//             }
//         }
//     }

//     private void ModifyPhysics()
//     {
//         bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

//         if (onGround) {
//             if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
//             {
//                 rb.drag = linearDrag;
//             } else
//             {
//                 rb.drag = 0f;
//             }
//             rb.gravityScale = 0;
//         }
//         else
//         {
//             rb.gravityScale = gravity;
//             rb.drag = linearDrag * 0.5f;
//             if(rb.velocity.y < 0)
//             {
//                 rb.gravityScale = gravity * fallMultiplier;
//             } else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
//             {
//                 rb.gravityScale = gravity * (fallMultiplier / 2);
//             }
//         }
//     }

//     private void Jump()
//     {
//         float jump = (Mathf.Abs(rb.velocity.x) / maxSpeed);
//         rb.velocity = new Vector2(rb.velocity.x, 0);
//         rb.AddForce(Vector2.up * (jumpForce + jump), ForceMode2D.Impulse);

//         jumpTimer = 0;
//     }

//     private void OnDrawGizmos()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
//         Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
//     }
// }