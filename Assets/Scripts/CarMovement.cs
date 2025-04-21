using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarMovement : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Transform wayPoints;
    private int currentWayPoint;
    private Vector3 startingPosition;

    [Header("Components")]
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        startingPosition = transform.position;
        agent.SetDestination(wayPoints.GetChild(0).position);

    }
    private void Update()
    {
        if (agent.remainingDistance < 0.5f)
        {
            currentWayPoint++;
            if (currentWayPoint >= wayPoints.childCount)
            {
                // Rozpocznij korutynê z opóŸnieniem
                //transform.rotation = Quaternion.Euler(0, 180, 90);
                StartCoroutine(ReturnToStartAfterDelay(2f));
                return;
            }
            agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);
        }
        ProperDirectionRotation();
    }

    private IEnumerator ReturnToStartAfterDelay(float delay)
    {
        // Wstrzymaj wykonanie na okreœlony czas
        yield return new WaitForSeconds(delay);

        
        // Przenieœ samochód do punktu pocz¹tkowego
        transform.position = startingPosition;
        currentWayPoint = 0;

        // Ustaw nowy cel dla NavMeshAgent
        agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);
    }
    private void ProperDirectionRotation()
    {
        if (agent.pathPending == false && agent.remainingDistance > 0.1f)
        {
            Vector3 direction = agent.steeringTarget - transform.position; // Direction to the next waypoint
            direction.y = 0; // Ignore vertical rotation
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction); // Rotate the car to face the waypoint
            }
        }
            
    }

}

