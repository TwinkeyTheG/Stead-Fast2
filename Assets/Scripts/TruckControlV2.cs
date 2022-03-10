using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TruckControlV2 : MonoBehaviour
{


    private Rigidbody2D rb;

    private AudioSource myAud;

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
        if (collision.gameObject.layer == LayerMask.NameToLayer("test"))
        {
            if (collision.gameObject.CompareTag("House"))
            {
                SceneManager.LoadScene("Win");
            }
        }

    }
}
