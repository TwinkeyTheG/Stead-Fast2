using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;

public class Win : MonoBehaviour
{
    public TMP_Text Earned;
    public TMP_Text Lost;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
    }
    //Updates the order information.
    public void OrderText()
    {
        myOrder.text = "Money Earned: " +  + "\n\n\n\n" +
              "Lost Money: " + Type[OrderNumber].BoxCircNum + "\n" +
               ;
    }
    //updates the text of the canvas of the player
    public void changeText()
    {
        OrderText();
        UpdateOrder.AddListener(OrderText);
    }
}
