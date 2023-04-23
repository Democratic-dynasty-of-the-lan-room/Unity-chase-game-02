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
    [SerializeField] GameObject Enemy;

    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text.enabled = false;
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
            text.enabled = true;
            EnemyController.enabled = false;
            navMeshAgent.enabled = false;

        }
    }

}
