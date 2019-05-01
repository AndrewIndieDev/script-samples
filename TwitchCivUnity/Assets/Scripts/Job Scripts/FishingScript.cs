using UnityEngine;
using System.Collections;

public class FishingScript : JobScript
{

    public PlayerState state = PlayerState.FISHING_IDLE;
    public GameObject targetFishingSpot;

    public override void DoJob()
    {
        switch (state)
        {
            case PlayerState.FISHING_IDLE:
                GetNode();
                break;

            case PlayerState.FISHING_PICKING_SPOT:

                break;

            case PlayerState.FISHING_GOTO_SPOT:
                if (!agent.hasPath)
                {
                    animation.Play("Walk");
                    agent.SetDestination(targetFishingSpot.transform.position);
                }
                if (Vector3.Distance(transform.position, targetFishingSpot.transform.position) < 2f)
                {
                    animation.Play("FishingCastRod");
                    agent.ResetPath();
                    state = PlayerState.FISHING_FISH;
                    StartCoroutine(CatchFish(Random.Range(5,10)));
                }
                break;

            case PlayerState.FISHING_FISH:

                break;

            case PlayerState.FISHING_FIND_STOCKPILE:
                GetNearestStockpile();
                if (nearestStockpile != null)
                {
                    animation.Play("CarryItem");
                    agent.SetDestination(nearestStockpile.transform.position);
                    state = PlayerState.FISHING_RETURN;
                }
                else
                    Debug.LogError("I cant find a stockpile, HALP");
                break;

            case PlayerState.FISHING_RETURN:
                if (Vector3.Distance(transform.position, nearestStockpile.transform.position) < 2f)
                {
                    animation.Play("DropItemStart");
                    agent.ResetPath();
                    DropResources(nearestStockpile);
                    StartCoroutine(ReturnToIdle(5));
                    state = PlayerState.FISHING_WAIT;
                }
                break;
        }
    }

    void GetNode()
    {
        state = PlayerState.FISHING_PICKING_SPOT;
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("FishingSpot");
        if (nodes.Length > 0)
        {
            targetFishingSpot = nodes[Random.Range(0, nodes.Length)];
            agent.ResetPath();
            state = PlayerState.FISHING_GOTO_SPOT;
        }
        else
        {
            StartCoroutine(ReturnToIdle(5));
        }
    }

    IEnumerator CatchFish(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        animation.Play("FishingCatchFish");
        StartCoroutine(CatchingAnimation());
    }

    IEnumerator CatchingAnimation()
    {
        yield return new WaitForSeconds(3);
        playerScript.AddItems(ItemType.FISH, 1);
        CreateVisual();
        yield return new WaitForSeconds(2);
        agent.ResetPath();
        state = PlayerState.FISHING_FIND_STOCKPILE;
    }

    IEnumerator ReturnToIdle(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        state = PlayerState.FISHING_IDLE;
        playerScript.JobFinished();
    }
}
