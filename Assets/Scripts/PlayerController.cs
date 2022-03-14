/*****************************************
 * Edited by: Ryan Scheppler
 * Last Edited: 1/27/2021
 * Description: This should be added to the player in a simple 2D platformer 
 * *************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //audio clips
    public AudioClip footstep;
    //Game manager Object
    GameManager Data;
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
    //How many of each box is on the truck
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
    public AudioClip dropBox1;
    public AudioClip dropBox2;
    public AudioClip dropBox3;
    public AudioClip dropBox4;
    public AudioClip boxPickup1;
    public AudioClip boxPickup2;

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
    public GameObject terminal;

    
    public GameObject Triangle;
    public GameObject Rectangle;
    public GameObject Circle;
    private Vector3 TrianglePos = new Vector3(25.56f, 6.5f, 0);
    private Vector3 RectanglePos = new Vector3(24.51f, -5.55f, 0);
    private Vector3 CirclePos = new Vector3(-8.3f, 1.3f, 0);
    private bool IsTri = false;
    private bool IsRect = false;
    private bool IsCirc = false;
    private bool InstantiationNeed = true;
    private Vector3 CurrentPos = new Vector3();

    // Start is called before the first frame update
    void Start()
    {

        myRb = GetComponent<Rigidbody2D>();
        myAud = GetComponent<AudioSource>();
        myAnim = GetComponent<Animator>();
        jumps = extraJumps;
        RespawnPoint = transform.position;
    }

    //Update is called once per frame
    private void Update()
    {
        
        if (InstantiationNeed == true)
        {
            Place();
        }
        if (Input.GetKeyDown(KeyCode.R) && HasBox == true)
        {
            myAnim.SetBool("HasBox", false);
            DropBox();
        }
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
        else if (Input.GetAxisRaw("Jump") == 0)
        {
            jumpPressed = false;
            jumpTimer = 0;
        }
        else if (jumpPressed == true && jumpTimer < jumpTime)
        {
            print("jumpTimer?");
            
            jumpTimer += Time.deltaTime;
            myRb.drag = airDrag;
            myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
            jumpPressed = true;
        }
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
            if (moveInputV > 0)
            {
                myRb.AddForce(new Vector2(0, climbSpeed));
            }
            else if (moveInputV < 0)
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
        if (moveInputH == 0)
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
            myRb.AddForce(new Vector2(moveInputH * speed, 0));
        }
        else
        {
            myRb.drag = airDrag;
            myRb.AddForce(new Vector2(moveInputH * airSpeed, 0));
        }
        //check if we need to flip the player direction
        if (facingRight == false && moveInputH > 0)
            Flip();
        else if (facingRight == true && moveInputH < 0)
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

    void Place()
    {
        InstantiationNeed = false;
        if (IsTri == false)
        {
            IsTri = true;
            Instantiate(Triangle, TrianglePos, Quaternion.identity);
            myAud.PlayOneShot(dropBox1);
        }
        if (IsCirc == false)
        {
            IsCirc = true;
            Instantiate(Circle, CirclePos, Quaternion.identity);
            myAud.PlayOneShot(dropBox2);
        }
        if (IsRect == false)
        {
            IsRect = true;
            Instantiate(Rectangle, RectanglePos, Quaternion.identity);
            myAud.PlayOneShot(dropBox3);
        }
    }

    void DropBox()
    {
        HasBox = false;
        CurrentPos = transform.TransformPoint(Vector3.zero);
        if (HasTriangle == true)
        {
            HasTriangle = false;
            Instantiate(Triangle, CurrentPos, Quaternion.identity);
            myAud.PlayOneShot(dropBox1);
        }
        else if (HasCircle == true)
        {
            HasCircle = false;
            Instantiate(Circle, CurrentPos, Quaternion.identity);
            myAud.PlayOneShot(dropBox2);
        }
        else if (HasRectangle == true)
        {
            HasRectangle = false;
            Instantiate(Rectangle, CurrentPos, Quaternion.identity);
            myAud.PlayOneShot(dropBox3);
        }
    }

    //the player ho0lding a package system
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Triangle") || collision.gameObject.CompareTag("Rectangle") || collision.gameObject.CompareTag("Circle"))
        {
            Box = collision.gameObject;

            if (HasBox == false && Input.GetKey(KeyCode.E))
            {
                if (collision.gameObject.CompareTag("Triangle"))
                {
                    print("Triangle recieved");
                    HasTriangle = true;
                    Destroy(Box);
                    myAnim.SetBool("HasBox", true);
                    HasBox = true;
                    myAud.PlayOneShot(boxPickup1);
                    InstantiationNeed = true;
                    IsTri = false;
                }
                else if (collision.gameObject.CompareTag("Rectangle"))
                {
                    print("Rectangle recieved");
                    HasRectangle = true;
                    Destroy(Box);
                    myAnim.SetBool("HasBox", true);
                    HasBox = true;
                    myAud.PlayOneShot(boxPickup1);
                    InstantiationNeed = true;
                    IsRect = false;
                }
                else if (collision.gameObject.CompareTag("Circle"))
                {
                    print("Circle recieved");
                    HasCircle = true;
                    Destroy(Box);
                    myAnim.SetBool("HasBox", true);
                    HasBox = true;
                    myAud.PlayOneShot(boxPickup2);
                    InstantiationNeed = true;
                    IsCirc = false;
                }
            }
        }
        if (HasBox == true && collision.gameObject.CompareTag("Truck") && Input.GetKey(KeyCode.E))
        {
            myAnim.SetBool("HasBox", false);
            HasBox = false;
            myAud.PlayOneShot(dropBox4);
            if (HasTriangle == true)
            {              
                Data.Triangles++;
                HasTriangle = false;
            }
            else if (HasRectangle == true)
            {
                Data.Rectangles++;
                HasRectangle = false;
            }
            else if (HasCircle == true)
            {
                Data.Circles++;
                HasCircle = false;
            }
        }

    }
    //to update Order display when colliding with terminal
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Terminal"))
        {
            Data.displayOn = true;
        }
        for(int i = 1; i <= 2; i++)
        {
            if (collision.gameObject.CompareTag(i.ToString()) && i == Data.Type[Data.OrderNumber].houseNum)
            {

            }
        }
        //keeps track of how many boxes will be delivered next.
        /*else
        {

        }*/

    }
    public void PlayFoot()
    {
        myAud.PlayOneShot(footstep);
    }
    //function to subtract values needed to be delivered to the houses 
    /*public int deliverPackages(int package, int packageRequired)
    {

    }*/
}