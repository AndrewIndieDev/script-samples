using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAttachment : MonoBehaviour
{

    public float radius = 0.05f;
    public LayerMask blocksLayer;

	// Use this for initialization
	void Start ()
	{
	    VRInputManager.inputEventTriggerRDown += RemoveBlock;

	    MenuManager.manager.SetMenu(MenuManager.EMenuType.Remove);
    }

    void OnDestroy()
    {
        VRInputManager.inputEventTriggerRDown -= RemoveBlock;
    }

    void RemoveBlock()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, radius, blocksLayer);
        GameObject nearestGO = null;
        float nearest = Mathf.Infinity;
        foreach (var index in cols)
        {
            float dist = Vector3.Distance(index.transform.position, transform.position);
            if (dist < nearest)
            {
                nearest = dist;
                nearestGO = index.gameObject;
            }
        }

        if (nearestGO != null)
        {
            Destroy(nearestGO);
        }
    }
}
