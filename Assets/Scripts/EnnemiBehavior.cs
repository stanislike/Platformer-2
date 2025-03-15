using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D bc;
    private int directionMovement;

    [SerializeField] private float speed = 2f;
    [SerializeField] private string obstacleTag = "ground";
    //[SerializeField] private string playerTag = "Player";
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        directionMovement = -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 move = Vector2.right * speed * directionMovement;
        move.y = rb.linearVelocity.y;

        rb.linearVelocity = move;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == obstacleTag)
        {
            Vector2 normal = other.GetContact(0).normal;
            Debug.DrawRay(other.GetContact(0).point, normal, Color.red, 1f);

            if (normal.x > 0)
            {
                directionMovement = 1;
            }
            
            else if (normal.x < 0)
            {
                directionMovement = -1;
            }
        }

        if (other.gameObject.tag == "Player" )
        {
            animator.enabled = false;
        }
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == obstacleTag)
        {
            Vector2 normal = -rb.linearVelocity.normalized ;
            Debug.DrawRay(other.ClosestPoint(gameObject.transform.position), normal, Color.red, 1f);

            if (normal.x > 0 && bc.IsTouching(other))
            {
                directionMovement = 1;
            }
            
            else if (normal.x < 0 && bc.IsTouching(other))
            {
                directionMovement = -1;
            }
        }
    }
}
