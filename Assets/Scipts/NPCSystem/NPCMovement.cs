using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private float maxRange = 1000.0f;
    [SerializeField] private float minRange = 10.0f;
    private float range;
    private bool isWalking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        isWalking = true;
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            isWalking = false;
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

    public bool IsWalking()
    {
        return isWalking;
    }
}
