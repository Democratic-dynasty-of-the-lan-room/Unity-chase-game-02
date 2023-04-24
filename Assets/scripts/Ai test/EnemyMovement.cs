using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public Transform door;
    NavMeshAgent agent;
    public float overcompensationAmount = 2f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Calculate a point in front of the player
        Vector3 predictedPoint = player.position + player.GetComponent<Rigidbody>().velocity * Time.deltaTime;
        
        // Calculate the direction from the predicted position to the door
        Vector3 direction = door.position - predictedPoint;
        
        // Calculate a perpendicular direction for overcompensation
        Vector3 perpendicularDirection = new Vector3(-direction.z, 0, direction.x).normalized;
        
        // Calculate a point in front of the player that is closer to the door
        Vector3 targetPoint = predictedPoint + direction.normalized * agent.speed * Time.deltaTime + perpendicularDirection * overcompensationAmount;
        Vector3 towardsDoor = door.position - targetPoint;
        targetPoint += towardsDoor.normalized * 2;

        // Move the enemy towards the target point
        agent.SetDestination(targetPoint);
    }
}