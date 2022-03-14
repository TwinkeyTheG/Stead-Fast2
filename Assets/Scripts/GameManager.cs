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
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //orders completed
    public int CompletedOrders = 0;
    //amount earned by the player
    public int MoneyEarned = 0;
    //Amount Lost
    //public int Lost = 0;
    //the amount of strikes you have
    //int strikes = 0;
    //Keepstrack of the time of day which is 12 mins per shift
    public float ShiftDur = 720;
    //These variables hold the values of how many packages are on the truck
    public int Triangles = 0;
    public int Rectangles = 0;
    public int Circles = 0;
    //Order Data variables
    public int OrderNumber = 0;
    public bool displayOn;
    //structure for orders
    public struct Orders
    {
        public int houseNum;
        public int BoxTriNum;
        public int BoxCircNum;
        public int BoxRectNum;
    }
    // amount of orders is 6 for now
    public Orders[] Type = new Orders[2];

    public bool begin = true;

    //Order updating stuff
    public static UnityEvent UpdateOrder = new UnityEvent();
    public TMP_Text myOrder;
    //allow this component to be grabbed from anywhere and make sure only one exists
    TruckControlV2 Truck;
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
        displayOn = false;
        if (begin == true)
        {
            //populate the array with 0 for each box type
            for (int i = 0; i < 2; i++)
            {
                Type[i].houseNum = 0;
                Type[i].BoxTriNum = 0;
                Type[i].BoxCircNum = 0;
                Type[i].BoxRectNum = 0;
            }
            for (int i = 0; i < 2; i++)
            {
                Type[i].houseNum = i + 1;
                Type[i].BoxTriNum = Random.Range(0, 5);
                Type[i].BoxCircNum = Random.Range(0, 5);
                Type[i].BoxRectNum = Random.Range(0, 5);
            }
            begin = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int Tri = Type[OrderNumber].BoxTriNum, Cir = Type[OrderNumber].BoxCircNum, Rect = Type[OrderNumber].BoxCircNum;
        //will check if order was complete
        if (Tri == 0 && Cir == 0 && Rect == 0)
        {
            OrderNumber++;
        }
        if(Truck.gas <= 0)
        {
            SceneManager.LoadScene("LoseScreen");
        }
        /*if(ShiftDur <= 0 && CompletedOrders < 8)
        {
            SceneManager.LoadScene("Win");
            Lost += 100;
        }
        else
        {
            SceneManager.LoadScene("Win");
        }
        ShiftDur -= Time.deltaTime;*/
        while(displayOn == true)
        {
            changeText();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        /*if(Tri == 0 && Cir == 0 && Rect == 0)
        {
            PackageDelivered = true;
            CompletedOrders++;
        }*/
    }
    public static void ResetGame()
    {
        score = 0;
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
}
