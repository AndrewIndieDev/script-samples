using System.Collections;
using UnityEngine;

public class WoodcutterScript : JobScript
{

    public PlayerState state = PlayerState.WOODCUTTING_IDLE;
    public GameObject targetTree;
    private float cutTreeTime = 5;

	public override void DoJob()
    {
	    switch (state)
        {
            case PlayerState.WOODCUTTING_IDLE:
                GetTree();
                break;

            case PlayerState.WOODCUTTING_PICKING_TREE:
                
                break;

            case PlayerState.WOODCUTTING_GOTO_TREE:
                if (!agent.hasPath)
                {
                    animation.Play("Walk");
                    agent.SetDestination(targetTree.transform.position);
                }
                if (Vector3.Distance(transform.position, targetTree.transform.position) < 3f)
                {
                    animation.Play("WoodcutterStart");
                    agent.ResetPath();
                    state = PlayerState.WOODCUTTING_CUTTING_TREE;
                    StartCoroutine(CutTree(cutTreeTime));
                }
                break;

            case PlayerState.WOODCUTTING_CUTTING_TREE:

                break;

            case PlayerState.WOODCUTTING_RETURN:
                if (!agent.hasPath)
                {
                    GetNearestStockpile();
                    if (nearestStockpile != null)
                    {
                        animation.Play("CarryItem");
                        CreateVisual();
                        agent.SetDestination(nearestStockpile.transform.position);
                    }
                    else
                        Debug.LogError("I cant find a stockpile, HALP");
                }
                if (Vector3.Distance(transform.position, nearestStockpile.transform.position) < 2f)
                {
                    animation.Play("DropItemStart");
                    agent.ResetPath();
                    DropResources(nearestStockpile);
                    StartCoroutine(ReturnToIdle(5));
                    state = PlayerState.WOODCUTTING_WAIT;
                }
                break;
        }
	}

    void GetTree()
    {
        state = PlayerState.WOODCUTTING_PICKING_TREE;
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        float dist = Mathf.Infinity;
        if (trees.Length > 0)
        {
            for (int i = 0; i < trees.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, trees[i].transform.position);
                if (distance < dist)
                {
                    dist = distance;
                    targetTree = trees[i];
                }
            }
            targetTree.tag = "OccupiedTree";
            agent.ResetPath();
            state = PlayerState.WOODCUTTING_GOTO_TREE;
        }
        else
        {
            StartCoroutine(ReturnToIdle(5));
        }
    }

    IEnumerator ReturnToIdle(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        state = PlayerState.WOODCUTTING_IDLE;
        playerScript.JobFinished();
    }

    IEnumerator CutTree(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        targetTree.AddComponent<TreeFall>();
        playerScript.AddItems(ItemType.LOG, 5);
        agent.ResetPath();
        state = PlayerState.WOODCUTTING_RETURN;
    }
}
