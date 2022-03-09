/*****************************************
 * Edited by: Ryan Scheppler
 * Last Edited: 1/27/2021
 * Description: Should be on it's own game object likely alone, only one will exist at a time, handles variables and events more global affecting
 * *************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class GameManager : MonoBehaviour
{
    //Terminal script Object
    Terminal myData;
    //Order updating stuff
    public static UnityEvent UpdateOrder = new UnityEvent();
    public TMP_Text myOrder;
    //allow this component to be grabbed from anywhere and make sure only one exists
    public static GameManager Instance;
    //event to listen to for the score change
    public static UnityEvent ScoreUpdate = new UnityEvent();
    //score property and int behind it
    private static int _score = 0;
    public static int score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            ScoreUpdate.Invoke();
        }
    }


    //when made make sure this is the only manager, and make the manager persistant through levels
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(myData.displayOn == true)
        {
            OrderText();
            changeText();
        }
    }
    //Updates the order information.
    public void OrderText()
    {
        myOrder.text = "House: " + myData.Type[myData.OrderNumber].houseNum + "\n" +
              "Circle Box(es): " + myData.Type[myData.OrderNumber].BoxCircNum + "\n" +
               "Triangle Box(es): " + myData.Type[myData.OrderNumber].BoxTriNum + "\n" +
               "Rectangle Box(es): " + myData.Type[myData.OrderNumber].BoxRectNum + "\n";
    }
    //updates the text of the canvas of the player
    public void changeText()
    {
        OrderText();
        UpdateOrder.AddListener(OrderText);
    }
    public static void ResetGame()
    {
        score = 0;
    }

}
