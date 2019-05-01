using UnityEngine;
using System.Collections;

public class MinerScript : JobScript
{

    public PlayerState state = PlayerState.MINING_IDLE;
    public GameObject targetNode;
    private float mineNodeTime = 5;

    public override void DoJob()
    {
        switch (state)
        {
            case PlayerState.MINING_IDLE:
                GetNode();
                break;

            case PlayerState.MINING_PICKING_NODE:

                break;

            case PlayerState.MINING_GOTO_NODE:
                if (!agent.hasPath)
                {
                    animation.Play("Walk");
                    agent.SetDestination(targetNode.transform.position);
                }
                if (Vector3.Distance(transform.position, targetNode.transform.position) < 2f)
                {
                    animation.Play("MinerStart");
                    agent.ResetPath();
                    state = PlayerState.MINING_MINE;
                    StartCoroutine(MineNode(mineNodeTime));
                }
                break;

            case PlayerState.MINING_MINE:

                break;

            case PlayerState.MINING_FIND_STOCKPILE:
                GetNearestStockpile();
                if (nearestStockpile != null)
                {
                    animation.Play("CarryItem");
                    CreateVisual();
                    agent.SetDestination(nearestStockpile.transform.position);
                    state = PlayerState.MINING_RETURN;
                }
                else
                    Debug.LogError("I cant find a stockpile, HALP");
                break;

            case PlayerState.MINING_RETURN:
                if (Vector3.Distance(transform.position, nearestStockpile.transform.position) < 2f)
                {
                    animation.Play("DropItemStart");
                    agent.ResetPath();
                    DropResources(nearestStockpile);
                    StartCoroutine(ReturnToIdle(5));
                    state = PlayerState.MINING_WAIT;
                }
                break;
        }
    }

    void GetNode()
    {
        state = PlayerState.MINING_PICKING_NODE;
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("MiningNode");
        if (nodes.Length > 0)
        {
            targetNode = nodes[Random.Range(0, nodes.Length)];
            agent.ResetPath();
            state = PlayerState.MINING_GOTO_NODE;
        }
        else
        {
            StartCoroutine(ReturnToIdle(5));
        }
    }

    IEnumerator MineNode(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        playerScript.AddItems(ItemType.STONE, 5);
        agent.ResetPath();
        state = PlayerState.MINING_FIND_STOCKPILE;
    }

    IEnumerator ReturnToIdle(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        state = PlayerState.MINING_IDLE;
        playerScript.JobFinished();
    }
}
