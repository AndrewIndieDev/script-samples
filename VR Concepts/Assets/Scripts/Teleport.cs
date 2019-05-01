using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private bool isTeleporting;
    private LineRenderer lr;
    private Vector3 pos;
    private bool hittingSomething = false;

    // Use this for initialization
    void Start ()
	{
	    VRInputManager.inputEventTouchpadRDown += TeleportStart;
	    VRInputManager.inputEventTouchpadRUp += TeleportEnd;
	    lr = gameObject.AddComponent<LineRenderer>();
	    lr.startWidth = 0.05f;
	    lr.endWidth = 0.05f;
        lr.material = new Material(Shader.Find("Standard"));
	    lr.enabled = false;
	}

    void OnDestroy()
    {
        VRInputManager.inputEventTouchpadRDown -= TeleportStart;
        VRInputManager.inputEventTouchpadRUp -= TeleportEnd;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    lr.enabled = false;
	    if (isTeleporting)
	    {
	        lr.SetPosition(0, transform.position);
	        RaycastHit hit;
	        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(60, transform.right) * transform.forward, out hit, 20))
	        {
	            if (hit.transform.gameObject.CompareTag("Teleportable"))
	            {
                    hittingSomething = true;
                    pos = hit.point;
	                lr.enabled = true;
                    lr.SetPosition(1, pos);
	            }
	            else
	            {
                    hittingSomething = false;
                }
	        }
	        else
	        {
                hittingSomething = false;
            }
	    }
	}

    void TeleportStart()
    {
        //if (VRInputManager.GetTouchPosR().y > 0)
        //    isTeleporting = true;
    }

    void TeleportEnd()
    {
        isTeleporting = false;
        if (hittingSomething)
        {
            transform.root.transform.position = pos;
        }
    }
}
