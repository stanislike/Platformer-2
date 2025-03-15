using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
    private GameMaster gm;
    
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    [SerializeField] private Collider2D c2D;
    [SerializeField] private LayerMask groundLayer;
    private Vector2 move = new Vector2(0, 0);
    private bool jumpRegistered;
    private Animator animator;
    private CharacterController2D cc2D;
    public Spawner spawner;
    //[SerializeField] List<GameObject> groundGO;
    
    [SerializeField] float jumpHeight;
    [SerializeField] float speed = 10f;
    //[SerializeField] bool isGrounded;
    [SerializeField] string groundTag = "ground";
    [SerializeField] private string ennemiTag = "ennemi";
    //[Range(0f, 1f)] [SerializeField] float groundNormalYThreshold = 0.7f;
    [SerializeField] float fallGravityModifier;
    [SerializeField] float smallJumpGravityModifier;
    [SerializeField] Vector2 bigJump;
    
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        //spawner = GetComponent<Spawner>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        cc2D = GetComponent<CharacterController2D>();
        //groundGO = new List<GameObject>();
        animator = GetComponent<Animator>();
        transform.position = gm.lastCheckpointPos;
    }

    private void Update()
    {
        if(IsGrounded() && Input.GetButtonDown("Jump"))
        {
            jumpRegistered = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gm.lastCheckpointPos.x = 0f;
            StartCoroutine(ReloadScene(0f));
        }
        
    }
    void FixedUpdate()
    {
        move = Vector2.right * speed * Input.GetAxis("Horizontal");
        move.y = rb.linearVelocity.y;

        if(jumpRegistered)
        {
            Debug.Log("Jump");
            move.y = Mathf.Sqrt(-2f * Physics2D.gravity.y * rb.gravityScale * jumpHeight);
            jumpRegistered = false;
            
            animator.SetTrigger("jump");
        }

        if (!IsGrounded())
        {
            if (move.y < 0f)
            {
                move.y += Physics2D.gravity.y * rb.gravityScale * fallGravityModifier * Time.deltaTime;
            }
            else if (!Input.GetButton("Jump"))
            {
                move.y += Physics2D.gravity.y * rb.gravityScale * smallJumpGravityModifier * Time.deltaTime;
            }
        }
        else
        {
            animator.SetTrigger("stopJump");
        }
        
        rb.linearVelocity = move;
        Debug.DrawRay(transform.position, rb.linearVelocity, Color.green);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == groundTag)
        {
            Vector2 normal = collision.GetContact(0).normal;
            Debug.DrawRay(collision.GetContact(0).point, normal, Color.green, 1f);
            
            /*
            if (normal.y > groundNormalYThreshold)
            {
                isGrounded = true;
                groundGO.Add(collision.gameObject);
                
                animator.SetTrigger("stopJump");
            }
            */

            if ( (normal.x > 0 || normal.x < 0) && normal.y <= 0 )
            {
                cc2D.enabled = false;
            }
            
        }

        if (collision.gameObject.tag == ennemiTag)
        {
            Vector2 normal = collision.GetContact(0).normal;
            Debug.DrawRay(collision.GetContact(0).point, normal, Color.blue, 1f);

            if (normal.y > 0) // le joueur tape un ennemi par le dessus
            {
                if (collision.rigidbody.isKinematic)
                {
                    collision.rigidbody.bodyType = 0;
                }
                //jumpRegistered = true;
                rb.AddForce(bigJump,ForceMode2D.Impulse); //Saut auto 
                spawner.nbEnnemi -= 1; //On donne l'info au spawner qu'on a un ennemi en moins
                collision.rigidbody.AddTorque(100f, ForceMode2D.Impulse);
                collision.collider.enabled = false; //Desactive le collider de l'ennemi
                Destroy(collision.gameObject, 3f); //Destruction de l'ennemi après un delay 
                
            }
            else // Sinon le joueur se fait avoir par un ennemi et meurt
            {
                rb.AddForce(new Vector2(0f,40f));
                rb.AddTorque(100f, ForceMode2D.Impulse);
                bc.enabled = false;
                StartCoroutine(ReloadScene(2f));
            }
        }

        if (collision.gameObject.tag == "Respawn")
        {
            StartCoroutine(ReloadScene(0f));
        }
    }
    
    /*
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == groundTag)
        {
            if (groundGO.Contains(collision.gameObject))
            {
                groundGO.Remove(collision.gameObject);
                if (groundGO.Count == 0)
                {
                    isGrounded = false;
                }
            }
        }
    }
    */
    
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(c2D.bounds.center, c2D.bounds.size, 0, Vector2.down, 0.1f, groundLayer);

        return hit.collider != null;
    }
    
    IEnumerator ReloadScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
