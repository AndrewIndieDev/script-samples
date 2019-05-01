using UnityEngine;
using System.Collections;

public class JobScript : MonoBehaviour
{
    [System.Serializable]
    public class VisualTransform
    {
        public Vector3 position, rotation, scale;
    }

    protected UnityEngine.AI.NavMeshAgent agent;
    protected Animator animation;
    public GameObject visualParent;
    public GameObject carryVisual;
    public VisualTransform visualTransform;
    protected PlayerBehaviour playerScript;
    public Stockpile nearestStockpile;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animation = GetComponent<Animator>();
        playerScript = GetComponent<PlayerBehaviour>();
    }

    public virtual void DoJob()
    {

    }

    public virtual void DropResources(Stockpile stockpile)
    {
        playerScript.DropResources(stockpile);
    }

    public void CreateVisual()
    {
        GameObject temp = (GameObject)Instantiate(carryVisual, visualParent.transform);
        temp.transform.localPosition = visualTransform.position;
        temp.transform.localRotation = Quaternion.Euler(visualTransform.rotation);
        temp.transform.localScale = visualTransform.scale;
        playerScript.carryItem = temp;
    }

    public void GetNearestStockpile()
    {
        Stockpile nearest = null;
        float dist = Mathf.Infinity;
        Stockpile[] stockpile = GameObject.FindObjectsOfType<Stockpile>();
        for (int i = 0; i < stockpile.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, stockpile[i].transform.position);
            if (distance < dist)
            {
                dist = distance;
                nearest = stockpile[i];
            }
        }
        nearestStockpile = nearest;
    }

}
