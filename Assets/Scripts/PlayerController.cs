﻿/*****************************************
 * Edited by: Ryan Scheppler
 * Last Edited: 1/27/2021
 * Description: This should be added to the player in a simple 2D platformer 
 * *************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //Interaction ranges
    //public BoxCollider2D pickupRange, loadUp;

    //other scripts that need to be accessed
    DeliverBox Boxes;

    //speed and movement variables
    public float speed;
    public float airSpeed;
    private float moveInputH;
    //grab this to adjust physics
    private Rigidbody2D myRb;

    //grabs gameobejct to destroy it and stores in this varible 
    private GameObject Box;
    public static bool HasBox;
    public static bool HasTriangle = false;
    public static bool HasRectangle = false;
    public static bool HasCircle = false;
    //used for checking what direction to be flipped
    private bool facingRight = true;

    //things for ground checking
    private bool isGrounded = false;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    //jump things
    public int extraJumps = 1;
    private int jumps;
    public float jumpForce;
    private bool jumpPressed = true;

    private float jumpTimer = 0;
    public float jumpTime = 0.2f;

    public float gravityScale = 5;

    public float groundDrag = 5;
    public float airDrag = 1;

    private AudioSource myAud;
    public AudioClip jumpNoise;
    /*public AudioClip jumpNoise;
    public AudioClip jumpNoise;
    public AudioClip jumpNoise;
    public AudioClip jumpNoise;
    public AudioClip jumpNoise;
    public AudioClip jumpNoise;*/

    //ladder things
    private bool isClimbing;
    public LayerMask whatIsLadder;
    public float ladderDist;
    private float moveInputV;
    public float climbSpeed;

    //Respawn info
    [HideInInspector]
    public Vector3 RespawnPoint = new Vector3();

    //animation
    private Animator myAnim;

    public Terminal panel;
    public GameObject terminal;

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAud = GetComponent<AudioSource>();
        myAnim = GetComponent<Animator>();
        Boxes = GetComponent<DeliverBox>();
        jumps = extraJumps;
        panel = terminal.GetComponent<Terminal>();
        RespawnPoint = transform.position;
    }

    //Update is called once per frame
    private void Update()
    {

        moveInputH = Input.GetAxisRaw("Horizontal");
        if (isGrounded == true)
        {
            jumps = extraJumps;
        }
        //check if jump can be triggered
        if (Input.GetAxisRaw("Jump") == 1 && jumpPressed == false && isGrounded == true && isClimbing == false)
        {
            myAud.PlayOneShot(jumpNoise);
            myRb.drag = airDrag;
            if ((myRb.velocity.x < 0 && moveInputH > 0) || (myRb.velocity.x > 0 && moveInputH < 0))
            {
                myRb.velocity = (Vector2.up * jumpForce);
            }
            else
            {
                myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
            }
            jumpPressed = true;
        }
        else if (Input.GetAxisRaw("Jump") == 1 && jumpPressed == false && jumps > 0 && isClimbing == false)
        {
            myAud.PlayOneShot(jumpNoise);
            myRb.drag = airDrag;
            if ((myRb.velocity.x < 0 && moveInputH > 0) || (myRb.velocity.x > 0 && moveInputH < 0))
            {
                myRb.velocity = (Vector2.up * jumpForce);
            }
            else
            {
                myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
            }
            jumpPressed = true;
            jumps--;
        }
        else if(Input.GetAxisRaw("Jump") == 0)
        {
            jumpPressed = false;
            jumpTimer = 0;
        }
        else if(jumpPressed == true && jumpTimer < jumpTime)
        {
            jumpTimer += Time.deltaTime;
            myRb.drag = airDrag;
            myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
            jumpPressed = true;
        }
        /*if(HasBox = true && Input.GetKeyDown(KeyCode.E))
        { 
            
        }*/
    }
    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {
        //check for ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        //set animators on ground
        myAnim.SetBool("OnGround", isGrounded);

        //ladder things

        moveInputV = Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("Jump");
        //check for the ladder if around the player
        RaycastHit2D hitInfo = Physics2D.Raycast(groundCheck.position, Vector2.up, ladderDist, whatIsLadder);
        
        //if ladder was found see if we are climbing, stop falling
        if (hitInfo.collider != null)
        {
            myRb.gravityScale = 0;
            isClimbing = true;
            if(moveInputV > 0)
            {
                myRb.AddForce(new Vector2(0, climbSpeed));
            }
            else if(moveInputV < 0)
            {
                myRb.AddForce(new Vector2(0, -climbSpeed));
            }
            else
            {
                myRb.velocity = new Vector2(myRb.velocity.x, 0);
            }
        }
        else
        {
            myRb.gravityScale = gravityScale;
            isClimbing = false;
        }
        
        //horizontal movement
        moveInputH = Input.GetAxisRaw("Horizontal");
        //animator settings
        if(moveInputH == 0)
        {
            myAnim.SetBool("Moving", false);
        }
        else
        {
            myAnim.SetBool("Moving", true);
        }

        if (isGrounded && !jumpPressed || isClimbing)
        {
            myRb.drag = groundDrag;
            myRb.AddForce(new Vector2(moveInputH * speed , 0));
        }
        else
        {
            myRb.drag = airDrag;
            myRb.AddForce(new Vector2(moveInputH * airSpeed  , 0));
        }
        //check if we need to flip the player direction
        if (facingRight == false && moveInputH > 0)
            Flip();
        else if(facingRight == true && moveInputH < 0)
        {
            Flip();
        }

    }
    //flip the player so sprite faces the other way
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Box = collision.gameObject;

        if (HasBox == false && Input.GetKeyDown(KeyCode.E))
        {
            if (collision.gameObject.CompareTag("Triangle"))
            {
                print("Triangle recieved");
                HasTriangle = true;
                Destroy(Box);
                myAnim.SetBool("HasBox", true);
                HasBox = true;
            }
            else if (collision.gameObject.CompareTag("Rectangle"))
            {
                print("Rectangle recieved");
                HasRectangle = true;
                Destroy(Box);
                myAnim.SetBool("HasBox", true);
                HasBox = true;
            }
            else if (collision.gameObject.CompareTag("Circle"))
            {
                print("Circle recieved");
                HasCircle = true;
                Destroy(Box);
                myAnim.SetBool("HasBox", true);
                HasBox = true;
            }
        }
        if (HasBox == true && collision.gameObject.CompareTag("Truck") && Input.GetKeyDown(KeyCode.E))
        {
            myAnim.SetBool("HasBox", false);
            HasBox = false;
            //Boxes.TruckCount++;
            if (HasTriangle == true)
            {
                Boxes.Triangles++;
                HasTriangle = false;
            }else if (HasRectangle == true)
            {
                Boxes.Rectangles++;
                HasRectangle = false;
            }else if (HasCircle == true)
            {
                Boxes.Circles++;
                HasCircle = false;
            }

        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        
    private void OnTriggerStay2D(Collider2D collision)
    {
        Box = collision.gameObject;

        if (Input.GetKeyDown(KeyCode.E) && collision.gameObject.CompareTag("Box"))
        {
            print("package recieved");
            Destroy(Box);
            myAnim.SetBool("HasBox", true);
            HasBox = true;
        }
        if (collision.gameObject.CompareTag("Truck") && Input.GetKeyDown(KeyCode.E))
        {
            myAnim.SetBool("HasBox", false);
            HasBox = false;
            Boxes.TruckCount++;
        }
    }
    
    
        Box = collision.gameObject;

        if (Input.GetKeyDown(KeyCode.E) && collision.gameObject.CompareTag("Box"))
        {
            print("package recieved");
            /*if (collision.gameObject.CompareTag("BoxTri"))
            {
                panel.Type[0].BoxTriNum += 1;
            }
            else if (collision.gameObject.CompareTag("BoxCirc"))
            {
                panel.Type[0].BoxCircNum += 1;
            }
            else if(collision.gameObject.CompareTag("BoxRect"))
            {
                panel.Type[0].BoxRectNum += 1;
            }*//*
            Destroy(Box);
            myAnim.SetBool("HasBox", true);
            HasBox = true;
        }*/
}