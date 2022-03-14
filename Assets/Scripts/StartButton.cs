/*****************************************
 * Edited by: Ryan Scheppler
 * Last Edited: 1/27/2021
 * Description: add to start button (or a level button or whatever) and set the onclick event to run the funciton in here.
 * *************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    GameManager man;
    //name of the scene to load on button click
    public string LevelToLoad = "BaseLevel";
    // add this function to the button onclick in the editor
    private void Start()
    {
        man = FindObjectOfType<GameManager>();
    }
    public void LevelLoad()
    {
        SceneManager.LoadScene(LevelToLoad);
        /*man.CompletedOrders = 0;
        man.Circles = 0;
        man.Triangles = 0;
        man.Rectangles = 0;
        man.ShiftDur = 720;
        man.ShiftDur = 720;
        man.begin = true;*/
    }

    public void ResetData()
    {
        GameManager.score = 0;
    }

}
