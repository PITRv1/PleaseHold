using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private float maxRange = 10.0f;
    private float minRange = 30.0f;
    private float range;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(transform.position, out point)) {
                agent.SetDestination(point);
            }
        }
    }

    bool RandomPoint(Vector3 center, out Vector3 result) 
    {
        range = Random.Range(minRange, maxRange);
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        float maxDistanceBetweenPoints = 1.0f;

        if (NavMesh.SamplePosition(randomPoint, out hit, maxDistanceBetweenPoints, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
