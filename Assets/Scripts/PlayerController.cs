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

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //DeliverBox Variables
    public bool PackageDelivered = false;
    public int Triangles = 0;
    public int Rectangles = 0;
    public int Circles = 0;
    //Order Data variables
    public int OrderNumber = 1, PackageNum = 1;
    private static int customers = 4;
    public bool displayOn = false;
    //structure for orders
    public struct Orders
    {
        public int houseNum;
        public int BoxTriNum;
        public int BoxCircNum;
        public int BoxRectNum;
    }
    // amount of orders is 6 for now
    public Orders[] Type = new Orders[customers];

    public bool begin = true;
    //Interaction ranges
    //public BoxCollider2D pickupRange, loadUp;

    //Order updating stuff
    public static UnityEvent UpdateOrder = new UnityEvent();
    public TMP_Text myOrder;

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
    public GameObject terminal;

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAud = GetComponent<AudioSource>();
        myAnim = GetComponent<Animator>();
        jumps = extraJumps;
        RespawnPoint = transform.position;
        if (begin == true)
        {
            //populate the array with 0 for each box type
            for (int i = 0; i < customers; i++)
            {
                Type[i].houseNum = 0;
                Type[i].BoxTriNum = 0;
                Type[i].BoxCircNum = 0;
                Type[i].BoxRectNum = 0;
            }
            for (int i = 0; i < customers; i++)
            {
                Type[i].houseNum = i + 1;
                Type[i].BoxTriNum = Random.Range(0, 5);
                Type[i].BoxCircNum = Random.Range(0, 5);
                Type[i].BoxRectNum = Random.Range(0, 5);
            }
            begin = false;
        }
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
        //will check if order was complete
        if (Type[OrderNumber].BoxCircNum == 0 && Type[OrderNumber].BoxTriNum == 0 && Type[OrderNumber].BoxRectNum == 0)
        {
            print("Successfully delivered!");
            PackageDelivered = true;
        }
        if (PackageDelivered == true)
        {
            OrderNumber++;
            PackageDelivered = false;
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

    //the player ho0lding a package system
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Triangle") || collision.gameObject.CompareTag("Rectangle") || collision.gameObject.CompareTag("Circle"))
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
        }
        if (HasBox == true && collision.gameObject.CompareTag("Truck") && Input.GetKeyDown(KeyCode.E))
        {
            myAnim.SetBool("HasBox", false);
            HasBox = false;
            if (HasTriangle == true)
            {
                Triangles++;
                HasTriangle = false;
            }
            else if (HasRectangle == true)
            {
                Rectangles++;
                HasRectangle = false;
            }
            else if (HasCircle == true)
            {
                Circles++;
                HasCircle = false;
            }
        }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
    }
    //to update Order display when colliding with terminal
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Terminal"))
        {
            changeText();
            displayOn = true;
        }
        //checks if the house has already recieved the sufficient amount of a box type and will return the message House Does Not need anymore of this box
        if (collision.gameObject.CompareTag("House") && ((Type[OrderNumber].BoxCircNum != 0 && collision.gameObject.CompareTag("Cir")) && (Type[OrderNumber].BoxTriNum != 0 && collision.gameObject.CompareTag("Tri")) && (Type[OrderNumber].BoxRectNum == 0 && collision.gameObject.CompareTag("Rect"))))
        {
            print("House still requires packages that you don't currently posses. Go back to the wearhouse and load up more of this package type.");
        }
        //keeps track of how many boxes will be delivered next.
        else
        {

        }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
    }
    //Updates the order information.
    public void OrderText()
    {
        myOrder.text = "House: " + Type[OrderNumber].houseNum + "\n" +
              "Circle Box(es): " + Type[OrderNumber].BoxCircNum + "\n" +
               "Triangle Box(es): " + Type[OrderNumber].BoxTriNum + "\n" +
               "Rectangle Box(es): " + Type[OrderNumber].BoxRectNum + "\n";
    }
    //updates the text of the canvas of the player
    public void changeText()
    {
        OrderText();
        UpdateOrder.AddListener(OrderText);
    }
    //function to subtract values needed to be delivered to the houses 
    /*public int deliverPackages(int package, int packageRequired)
    {

    }*/
}