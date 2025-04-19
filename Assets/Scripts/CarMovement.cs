using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarMovement : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Transform wayPoints;
    private int currentWayPoint;

    [Header("Components")]
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.updateRotation = false;

    }
    private void Update()
    {
        if (agent.remainingDistance < 0.5f)
        {
            currentWayPoint++;
            if (currentWayPoint >= wayPoints.childCount)
            {
                currentWayPoint = 0;
            }
            agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);
        }
        transform.rotation = Quaternion.Euler(0, 180, 90);

    }

}

