using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;

public class TruckControlV2 : MonoBehaviour
{
    //Gas the truck has
    public float gas = 70;

    //Object to store GameManagerData in
    GameManager Data;

    private Rigidbody2D rb;
    //Audio stuff
    private AudioSource myAud;
    public AudioClip Crash;
    /*public AudioClip jumpNoises;
    public AudioClip dropBox1s;
    public AudioClip dropBox2s;
    public AudioClip dropBox3s;
    public AudioClip dropBox4s;
    public AudioClip boxPickups1;
    public AudioClip boxPickup2s;*/

    //animation
    private Animator myAnim;

    public float Weight = 0;
    public float MaxWeight = 25;
    private float hInput = 0.0f;
    private float vInput = 0.0f;

    public float speed = 200.0f;
    public float turnRate = 3.0f;

    Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAud = GetComponent<AudioSource>();
        myAnim = GetComponent<Animator>();
        targetRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////////////////myAud.PlayOneShot(dropBox4);
        gas -= Time.deltaTime;
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
    }
    void FixedUpdate()
    {
        //Vector3 move = new Vector3(hInput, vInput, 0);
        if (hInput != 0)
        {
            if (hInput > 0)
            {
                rb.AddTorque(-turnRate);
            }
            if (hInput < 0)
            {
                rb.AddTorque(turnRate);
            }
        }
        
        if (vInput != 0.0f)
        {
            //targetRotation = Quaternion.LookRotation(Vector3.forward, move);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnRate);
            if (vInput > 0)
            {
                rb.AddRelativeForce(Vector3.up * speed * Time.fixedDeltaTime);
            }
            if (vInput < 0)
            {
                rb.AddRelativeForce(Vector3.down * speed * Time.fixedDeltaTime);
            }
            
        }
        // Get a left or right 90 degrees angle based on the rigidbody current rotation velocity
        float steeringRightAngle;
        if (rb.angularVelocity > 0)
        {
            steeringRightAngle = -90;
        }
        else
        {
            steeringRightAngle = 90;
        }
        // Find a Vector2 that is 90 degrees relative to the local forward direction (2D top down)
        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * Vector2.up;
        // Calculate the 'drift' sideways velocity from comparing rigidbody forward movement and the right angle to this.
        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(rightAngleFromForward.normalized));
        // Calculate an opposite force to the drift and apply this to generate sideways traction (aka tyre grip)
        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);
        rb.AddForce(rb.GetRelativeVector(relativeForce));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //stores the specified order details into integer values
        int TriBoxes = Data.Type[Data.OrderNumber].BoxTriNum, CircBoxes = Data.Type[Data.OrderNumber].BoxCircNum, RectBoxes = Data.Type[Data.OrderNumber].BoxRectNum;
        //keeps track of the remainder
        int Remainder = 0;
        if (collision.gameObject.layer == LayerMask.NameToLayer("House"))
        {
            //int to compare the house number of the order to
            int Location = 0;
            int.TryParse(collision.gameObject.tag, out Location);
            if(Location == Data.Type[Data.OrderNumber].houseNum)
            {
                //checks if the truck has more boxes than required in the order.
                if(Data.Triangles >= TriBoxes)
                {
                    //first finds the remainder
                    Remainder = Data.Triangles - Data.Type[Data.OrderNumber].BoxTriNum;
                    //then sets the remainder of the boxes to the amount in truck
                    Data.Triangles = Remainder;
                    //Finally elimnates the value from the order
                    Data.Type[Data.OrderNumber].BoxTriNum = 0;
                }
                //otherwise, if the amount in order is higher than amount in truck then...
                else
                {
                    //Leaves the remaining amount in order details
                    Data.Type[Data.OrderNumber].BoxTriNum -= Data.Triangles;
                    //then leaves 0 left in truck
                    Data.Triangles = 0;
                }
                //checks if the truck has more boxes than required in the order.
                if (Data.Circles >= CircBoxes)
                {
                    //first finds the remainder
                    Remainder = Data.Circles - Data.Type[Data.OrderNumber].BoxCircNum;
                    //then sets the remainder of the boxes to the amount in truck
                    Data.Circles = Remainder;
                    //Finally elimnates the value from the order
                    Data.Type[Data.OrderNumber].BoxCircNum = 0;
                }
                //otherwise, if the amount in order is higher than amount in truck then...
                else
                {
                    //Leaves the remaining amount in order details
                    Data.Type[Data.OrderNumber].BoxCircNum -= Data.Circles;
                    //then leaves 0 left in truck
                    Data.Circles = 0;
                }
                //checks if the truck has more boxes than required in the order.
                if (Data.Rectangles >= RectBoxes)
                {
                    //first finds the remainder
                    Remainder = Data.Rectangles - Data.Type[Data.OrderNumber].BoxRectNum;
                    //then sets the remainder of the boxes to the amount in truck
                    Data.Rectangles = Remainder;
                    //Finally elimnates the value from the order
                    Data.Type[Data.OrderNumber].BoxRectNum = 0;
                }
                //otherwise, if the amount in order is higher than amount in truck then...
                else
                {
                    //Leaves the remaining amount in order details
                    Data.Type[Data.OrderNumber].BoxRectNum -= Data.Rectangles;
                    //then leaves 0 left in truck
                    Data.Rectangles = 0;
                }
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject Destroyable = collision.gameObject;
        if(collision.gameObject.CompareTag("Bussin"))
        {
            Destroy(Destroyable);
            myAud.PlayOneShot(Crash);
            GameManager.score += 10;
            Data.MoneyEarned += 10;
        }
    }
}
