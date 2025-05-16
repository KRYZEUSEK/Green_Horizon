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
    private Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        startingPosition = transform.position;
        agent.SetDestination(wayPoints.GetChild(0).position);

        animator = GetComponent<Animator>();

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
        if (animator != null)
            animator.enabled = false;
        // Wstrzymaj wykonanie na okreœlony czas
        yield return new WaitForSeconds(delay);


        // Przenieœ samochód do punktu pocz¹tkowego
        transform.position = startingPosition;
        currentWayPoint = 0;

        // W³¹cz animacjê ponownie
        if (animator != null)
            animator.enabled = true;

        // Ustaw nowy cel dla NavMeshAgent
        
        agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);

    }
    private void ProperDirectionRotation()
    {
        if (agent.pathPending == false && agent.remainingDistance > 0.1f)
        {
            Vector3 direction = agent.steeringTarget - transform.position; // Kierunek do nastêpnego waypointu
            direction.y = 0; // Ignoruj rotacjê w osi Y (pionow¹)
            if (direction != Vector3.zero)
            {
                // Oblicz docelow¹ rotacjê
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Interpoluj rotacjê obiektu w kierunku docelowej rotacji
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, // Aktualna rotacja
                    targetRotation,     // Docelowa rotacja
                    Time.deltaTime * 5f // Prêdkoœæ rotacji (5f to przyk³adowa wartoœæ, mo¿esz j¹ dostosowaæ)
                );
            }
        }
    }

}

