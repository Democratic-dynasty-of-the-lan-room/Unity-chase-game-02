using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //If play is clicked load next scene
   public void PlayGame()
   {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   }

    //Quits game 
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
