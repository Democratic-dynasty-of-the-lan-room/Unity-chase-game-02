using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMenu : MonoBehaviour
{

    [SerializeField] GameObject restartMenu;

    // Start is called before the first frame update
    void Start()
    {
        // making sure RestartMenu is off at the start
        restartMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Restarts when you click on restart
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Loads menu scene
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
