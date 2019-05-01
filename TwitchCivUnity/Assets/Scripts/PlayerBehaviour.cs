using System.Collections.Generic;
using UnityEngine;

public enum JobID
{
    WOODCUTTER,
    MINER,
    FISHING
}

public enum PlayerState
{
    //fisherman
    FISHING_IDLE,
    FISHING_PICKING_SPOT,
    FISHING_GOTO_SPOT,
    FISHING_FISH,
    FISHING_FIND_STOCKPILE,
    FISHING_RETURN,
    FISHING_WAIT,

    //woodcutter
    WOODCUTTING_IDLE,
    WOODCUTTING_PICKING_TREE,
    WOODCUTTING_GOTO_TREE,
    WOODCUTTING_CUTTING_TREE,
    WOODCUTTING_FIND_STOCKPILE,
    WOODCUTTING_RETURN,
    WOODCUTTING_WAIT,

    //miner
    MINING_IDLE,
    MINING_PICKING_NODE,
    MINING_GOTO_NODE,
    MINING_MINE,
    MINING_FIND_STOCKPILE,
    MINING_RETURN,
    MINING_WAIT,
    
    //social
    SOCIAL_IDLE,
    SOCIAL_PICKING_PARTNER,
    SOCIAL_PICKING_ACTIVITY,
    SOCIAL_GOTO_ACTIVITY,
    SOCIAL_PLAYING
}

[System.Serializable]
public class Job
{
    public GameObject tool;
    public JobScript jobScript;
    public GameObject hand;
}

public class PlayerBehaviour : MonoBehaviour
{
    public string name;

    public List<ItemType> Inventory = new List<ItemType>();

    public Job[] jobs; //Job
    private JobScript job; //current job script
    public JobID currentJobID; //current ID of script (for player viewer)
    public GameObject carryItem; //item the player is carrying
    private GameObject currentTool = null;
    
	void Start ()
    {
        JobScript[] tempjobs = GetComponents<JobScript>();
	    for(int i = 0; i < jobs.Length; i++)
	    {
	        jobs[i].jobScript = tempjobs[i];
	    }
	    SetJob(currentJobID);
    }
	
	void Update ()
    {
        job.DoJob();
	}

    public void AddItems(ItemType type, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Inventory.Add(type);
        }
    }

    public void DropResources(Stockpile stockpile)
    {
        while (Inventory.Count > 0)
        {
            stockpile.inventory.Add(Inventory[0]);
            Inventory.Remove(Inventory[0]);
        }
        if (carryItem != null)
        {
            Destroy(carryItem);
        }
    }

    public void SetJob(JobID jobID)
    {
        job = jobs[(int)jobID].jobScript;
        if (currentTool != null)
        {
            Destroy(currentTool);
        }
        if (jobs[(int) jobID].tool != null)
        {
            GameObject temp = (GameObject)Instantiate(jobs[(int) jobID].tool);
            temp.transform.parent = jobs[(int) jobID].hand.transform;
            temp.transform.localPosition = jobs[(int)jobID].tool.transform.position;
            temp.transform.localRotation = jobs[(int)jobID].tool.transform.rotation;
            currentTool = temp;
        }
        currentJobID = jobID;
    }

    public void JobFinished()
    {
        SetJob(currentJobID);
    }
}
