using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;

public class Win : MonoBehaviour
{
    GameManager man;
    public TMP_Text Earned;
    public TMP_Text Lost;
    public UnityEvent ChangeText = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        changeText();
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
        Earned.text = "Money Earned: " + man.MoneyEarned;
        Lost.text = "Lost Money: " + man.Lost + "\n";
               
    }
    //updates the text of the canvas of the player
    public void changeText()
    {
        OrderText();
        ChangeText.AddListener(OrderText);
    }
}
