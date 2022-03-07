using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePlay : MonoBehaviour
{
    // Start is called before the first frame update
    void PauseGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }
    void ResumeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
