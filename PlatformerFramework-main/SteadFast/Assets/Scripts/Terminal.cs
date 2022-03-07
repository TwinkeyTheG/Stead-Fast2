using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/*********************************
 * Conner Vergara
 * 3/4/2021
 * Desc: Made a script that will overlay the orders to the player through a Text Mesh Pro canvas.
 * *******************************/

public class Terminal : MonoBehaviour
{
    TMP_Text myOrder;
    public int OrderNumber = 0, PackageNum;
    private static int customers = 4;
    //structure for orders
    /*public struct Orders
    {
        public int houseNum;
        public int BoxTriNum;
        public int BoxCircNum;
        public int BoxRectNum;
    }
    // amount of orders is 6 for now
    public Orders[] Type = new Orders[customers];

    public bool begin = true;*/
    
    // Start is called before the first frame update
    void Start()
    {
        myOrder = GetComponent<TMP_Text>();
        OrderNumber = 1;
        PackageNum = 1;
        /*if (begin == true)
        {   
            //populate the array with 0 for each box type
            for (int i = 0; i < customers; i++)
            {
                Type[i].houseNum = 0;
                Type[i].BoxTriNum = 0;
                Type[i].BoxCircNum = 0;
                Type[i].BoxRectNum = 0;
            }
            for(int i = 0; i < customers; i++)
            {
                Type[i].houseNum = i + 1;
                Type[i].BoxTriNum = Random.Range(0,5);
                Type[i].BoxCircNum = Random.Range(0,5);
                Type[i].BoxRectNum = Random.Range(0,5);
            }
            begin = false;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //A function for the player to interact with the terminal.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terminal"))
        {
            myOrder.text = "House: " + OrderNumber + "\n" +
                "Packages: " + PackageNum + "\n";

            /*myOrder.text = "House: " + Type[OrderNumber].houseNum + "\n" +
              "Circle Box(es): " + Type[OrderNumber].BoxCircNum + "\n" +
               "Triangle Box(es): " + Type[OrderNumber].BoxTriNum + "\n" +
               "Rectangle Box(es): " + Type[OrderNumber].BoxRectNum + "\n";*/
        }
    }
    //Ask Ryan about using a OnTriggerStay2D for this?
    /*private void OnTriggerStay2D(Collider2D collision)
    {
        
    }*/
}
