using UnityEngine;
using System.Collections;

public enum SocialActivity
{
    SOCIAL_IDLE,
    SOCIAL_GAME_THROWBALL,
    SOCIAL_GAME_TAG
}

public class SocialScript : JobScript {

    public PlayerState state = PlayerState.SOCIAL_IDLE;
    public SocialActivity socialState = SocialActivity.SOCIAL_IDLE;
    public bool looking = true;
    public float walkRadius = 40;
    public GameObject[] gamePartners;

    public override void DoJob()
    {
        switch(state)
        {
            case PlayerState.SOCIAL_IDLE:
                if (LookingForPartner())
                {
                    WalkingAround();
                }
                else
                {
                    state = PlayerState.SOCIAL_PICKING_ACTIVITY;
                }
                break;

            case PlayerState.SOCIAL_PICKING_PARTNER:

                break;

            case PlayerState.SOCIAL_PICKING_ACTIVITY:

                break;

            case PlayerState.SOCIAL_GOTO_ACTIVITY:

                break;

            case PlayerState.SOCIAL_PLAYING:

                break;
        }
    }

    bool LookingForPartner()
    {
        return true;
    }

    void WalkingAround()
    {
        if (!agent.hasPath)
        {
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;
            UnityEngine.AI.NavMeshHit hit;
            UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            Vector3 finalPosition = hit.position;

            agent.SetDestination(finalPosition);
        }
    }
}
