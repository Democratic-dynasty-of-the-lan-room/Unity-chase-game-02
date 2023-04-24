using UnityEngine;
using UnityEngine.AI;

public class EnemyController2 : MonoBehaviour
{
    public Transform player;
    public Transform door;
    public float aggressionLevel = 1.0f;
    public float catchModeDuration = 3.0f;
    public float catchModeChance = 5.0f;

    private bool isCatchingPlayer = false;
    private float catchModeTimer = 0.0f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        InvokeRepeating("RandomlySwitchToCatchMode", 3.0f, 3.0f);
    }

    void Update()
    {
        if (isCatchingPlayer) {
            catchModeTimer += Time.deltaTime;
            if (catchModeTimer > catchModeDuration) {
                isCatchingPlayer = false;
                GetComponent<NavMeshAgent>().SetDestination(initialPosition);
                transform.rotation = initialRotation;
            } else {
                Vector3 playerDirection = player.position - transform.position;
                float angle = Vector3.Angle(transform.forward, playerDirection);
                if (angle > 45.0f) {
                    transform.rotation = Quaternion.LookRotation(playerDirection);
                }
                GetComponent<NavMeshAgent>().SetDestination(player.position);
            }
        } else {
            Vector3 playerDirection = player.position - transform.position;
            Vector3 doorDirection = door.position - transform.position;

            float playerAngle = Vector3.Angle(transform.forward, playerDirection);
            float doorAngle = Vector3.Angle(transform.forward, doorDirection);

            bool playerBetween = playerAngle < doorAngle;
            bool doorBetween = doorAngle < playerAngle;

            if (Random.value < aggressionLevel) {
                if (playerBetween) {
                    Vector3 deviation = Random.insideUnitSphere * 2.0f * aggressionLevel;
                    Vector3 target = Vector3.Lerp(player.position, door.position, 0.5f) + deviation;
                    GetComponent<NavMeshAgent>().SetDestination(target);
                } else if (doorBetween) {
                    GetComponent<NavMeshAgent>().SetDestination(player.position);
                }
            }
        }
    }

    void RandomlySwitchToCatchMode()
    {
        if (Random.value < catchModeChance / 100.0f) {
            isCatchingPlayer = true;
            catchModeTimer = 0.0f;
            GetComponent<NavMeshAgent>().SetDestination(player.position);
        }
    }
}