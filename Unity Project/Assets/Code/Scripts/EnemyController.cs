using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class EnemyController : MonoBehaviour
{

    [SerializeField] GameObject Player;
  
    Transform target;
    NavMeshAgent agent;


    public float lookRadius = 10;

    public TextMeshProUGUI MissionFailed;

    //Check if objects collide for sword
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //If player touches chaser game end
            MissionFailed.enabled = true;
            Player.SetActive(false);
            this.enabled = false;

        }
    }




    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();

        // making sure mission failed is off at the start
        MissionFailed.enabled = false;

        //Making sure Player is set to true? I guess
        Player.SetActive(true);

        //this script I think
        this.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                //Attack
                //face target
                FaceTarget();
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3 (direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);  
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

    }

}
