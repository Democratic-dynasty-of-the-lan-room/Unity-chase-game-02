using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FinishScript : MonoBehaviour
{

    [SerializeField] GameObject Player;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] EnemyController EnemyController;

    //Getting all the menus
    [SerializeField] GameObject Enemy;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject FinishMenu;



    // Start is called before the first frame update
    void Start()
    {
        FinishMenu.SetActive(false);
        EnemyController.enabled = true;
        Player.SetActive(true);
        navMeshAgent.enabled = true;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Checks if the player is touching the door if so disable movment
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.SetActive(false);
            FinishMenu.SetActive(true);
            EnemyController.enabled = false;
            navMeshAgent.enabled = false;
            PauseMenu.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


        }
    }

}
