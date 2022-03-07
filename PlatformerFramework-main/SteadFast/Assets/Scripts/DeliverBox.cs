using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeliverBox : MonoBehaviour
{
    public Terminal myBox;
    public bool PackageDelivered = false;
    public int TruckCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        myBox = GetComponent<Terminal>();
    }

    // Update is called once per frame
    void Update()
    {
        //will check if order was complete
        /*if(myBox.Type[myBox.OrderNumber].BoxCircNum == 0 && myBox.Type[myBox.OrderNumber].BoxTriNum == 0 && myBox.Type[myBox.OrderNumber].BoxRectNum == 0)
        {
            print("Successfully delivered!");
            PackageDelivered = true; 
        }
        if (PackageDelivered == true)
        {
            myBox.OrderNumber++;
            PackageDelivered = false;
        }*/
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("House"))
        {

            //PackageDelivered = true;

            //checks if the house has already recieved the sufficient amount of a box type and will return the message House Does Not need anymore of this box
           /*if ((myBox.Type[myBox.OrderNumber].BoxCircNum == 0 && collision.gameObject.CompareTag("Cir")) || (myBox.Type[myBox.OrderNumber].BoxTriNum == 0 && collision.gameObject.CompareTag("Tri")) || (myBox.Type[myBox.OrderNumber].BoxRectNum == 0 && collision.gameObject.CompareTag("Rect")))
            {
                print("House Does Not need anymore of this box");
            }
            //keeps track of how many boxes will be delivered next.
            else
            {
                if(collision.gameObject.CompareTag("Cir"))
                {
                    myBox.Type[myBox.OrderNumber].BoxCircNum -= 1;
                }
                else if(collision.gameObject.CompareTag("Tri"))
                {
                    myBox.Type[myBox.OrderNumber].BoxTriNum -= 1;
                }
                else if(collision.gameObject.CompareTag("Rect"))
                {
                    myBox.Type[myBox.OrderNumber].BoxRectNum -= 1;
                }
            }*/
        }
    }
}
