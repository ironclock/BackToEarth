using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformerShipLand : MonoBehaviour
{
    [SerializeField] private LayerMask platformsLayerMask;
    private BoxCollider2D boxCollider2D;
    
    private bool landed;
    public GameObject player;
    void Start()
    {
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!landed)
        {
            Vector3 pos = transform.position;
            pos += ((5 * Time.smoothDeltaTime) * -transform.up);
            transform.position = pos;
        }
        
        if(!landed && IsGrounded())
        {
            landed = true;
            
            player.GetComponent<DogeController>().playerMovable = true;
            player.GetComponent<DogeController>().orientCharacter(1);
            player.GetComponent<DogeController>().respawnPos = transform.position;
            Vector3 pos = transform.position;
            pos += ((100 * Time.smoothDeltaTime) * transform.up);
            player.transform.position = pos;
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(25f, 50f);
        }
    }    

    private bool IsGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        return raycastHit2D.collider != null;
    }
}
