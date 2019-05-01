using UnityEngine;
using System.Collections;

public class Animal_Rabbit_Movement : MonoBehaviour {

    public enum RabbitState
    {
        IDLE,
        ROAMING,
        WAIT,
        FIND_TARGET
    }

    RabbitState state;
    public float radius = 10;
    UnityEngine.AI.NavMeshAgent agent;
    Animator animation;
    Vector3 startPos;
    Vector3 targetPos;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animation = GetComponent<Animator>();
        startPos = transform.position;
    }

    void Update()
    {
        switch (state)
        {
            case RabbitState.IDLE:
                StartCoroutine(RandomRoam(Random.Range(5f, 10f)));
                animation.Play("Animal_Rabbit_Idle");
                state = RabbitState.WAIT;
                break;

            case RabbitState.ROAMING:
                if (Vector3.Distance(transform.position, targetPos) < 1f)
                {
                    state = RabbitState.IDLE;
                    agent.ResetPath();
                }
                break;

            case RabbitState.WAIT:
                
                break;

            case RabbitState.FIND_TARGET:
                animation.Play("Animal_Rabbit_Jump");
                targetPos = GetRandomLocation(startPos);
                agent.SetDestination(targetPos);
                state = RabbitState.ROAMING;
                break;
        }
    }

    IEnumerator RandomRoam(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        state = RabbitState.FIND_TARGET;
    }

    Vector3 GetRandomLocation(Vector3 pos)
    {
        UnityEngine.AI.NavMeshHit hit;
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, radius, 1);
        return hit.position;
    }

}
