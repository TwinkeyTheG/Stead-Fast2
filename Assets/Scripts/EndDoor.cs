/*****************************************
 * Edited by: Ryan Scheppler
 * Last Edited: 1/27/2021
 * Description: Add this to an object the player can collide with to go to a new level
 * *************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDoor : MonoBehaviour
{
    public string LevelToLoad = "NeighborHood";
    // Start is called before the first frame update

    private void OnTriggerStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(LevelToLoad);
        }
    }
}
