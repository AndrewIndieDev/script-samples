using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementAttachment : MonoBehaviour
{

    public LayerMask menuBlockLayer;
    public LayerMask blockLayer;
    public LayerMask uiLayer;
    public Transform sphereCastObjectTransform;
    public float raycastRadius = 0.05f;
    private GameObject currentGrab;

	// Use this for initialization
	void Start ()
	{
	    VRInputManager.inputEventTriggerRDown += Grab;
	    VRInputManager.inputEventTriggerR += UpdateBlock;
        VRInputManager.inputEventTriggerRUp += Release;
	    MenuManager.manager.SetMenu(MenuManager.EMenuType.Blocks);
    }

    void OnDestroy()
    {
        VRInputManager.inputEventTriggerRDown -= Grab;
        VRInputManager.inputEventTriggerR -= UpdateBlock;
        VRInputManager.inputEventTriggerRUp -= Release;
    }

    void Grab()
    {
        Collider[] menuBlocks = Physics.OverlapSphere(sphereCastObjectTransform.position, raycastRadius, menuBlockLayer);
        Collider[] uiThings = Physics.OverlapSphere(sphereCastObjectTransform.position, raycastRadius, uiLayer);
        GameObject closest = null;
        float closestDist = Mathf.Infinity;
        foreach (var index in menuBlocks)
        {
            float dist = Vector3.Distance(index.transform.position, sphereCastObjectTransform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = index.transform.gameObject;
            }
        }

        GameObject closestui = null;
        float closestDistui = Mathf.Infinity;
        foreach (var index in uiThings)
        {
            float dist = Vector3.Distance(index.transform.position, sphereCastObjectTransform.position);
            if (dist < closestDistui)
            {
                closestDistui = dist;
                closestui = index.transform.gameObject;
            }
        }

        if (closestDist < closestDistui)
        {
            if (closest != null)
            {
                GameObject temp = Instantiate(closest);
                temp.transform.localScale = Vector3.one;
                temp.transform.position = sphereCastObjectTransform.position;
                temp.layer = LayerMask.NameToLayer("Blocks");

                currentGrab = temp;
            }
        }
        else
        {
            if (closestui != null)
            {
                try
                {
                    ((BlocksMenu) FindObjectOfType(typeof(BlocksMenu))).SetCategory(closestui.transform.gameObject
                        .GetComponent<MenuTab>().category);
                }
                catch
                {
                    //int i = 0;
                }
            }
        }
    }


    void UpdateBlock()
    {
        if (currentGrab == null) return;

        currentGrab.transform.position = new Vector3(Mathf.Round(sphereCastObjectTransform.position.x*10f)/10f, Mathf.Round(sphereCastObjectTransform.position.y * 10f) / 10f, Mathf.Round(sphereCastObjectTransform.position.z * 10f) / 10f);
        currentGrab.transform.rotation = Quaternion.Euler(0, Mathf.Round(transform.rotation.eulerAngles.y / 90) * 90, 0);

        /*currentGrab.transform.rotation = Quaternion.Euler(0, currentGrab.transform.rotation.eulerAngles.y, 0);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -currentGrab.transform.up, out hit, 0.05f, blockLayer))
        {
            currentGrab.transform.parent = null;
            //currentGrab.transform.rotation = hit.transform.rotation;
            Vector3 temp = hit.transform.worldToLocalMatrix.MultiplyPoint(transform.position);
            temp *= 10f;
            temp = new Vector3(Mathf.Round(temp.x), Mathf.Round(temp.y), Mathf.Round(temp.z)) / 10f;
            currentGrab.transform.position = hit.transform.localToWorldMatrix.MultiplyPoint(temp);
            for (int i = 0; i < 10; ++i)
            {
                Quaternion rotation = Quaternion.AngleAxis(Vector3.Angle(hit.transform.up, currentGrab.transform.up), Vector3.Cross(hit.transform.up, currentGrab.transform.up));
                currentGrab.transform.rotation *= rotation;
            }
            currentGrab.transform.parent = hit.transform;
            currentGrab.transform.localRotation = Quaternion.Euler(currentGrab.transform.localRotation.eulerAngles.x, Mathf.Round(currentGrab.transform.localRotation.eulerAngles.y/90f)*90f, currentGrab.transform.localRotation.eulerAngles.z);
            currentGrab.transform.parent = null;

        }
        else
        {
            currentGrab.transform.parent = transform;
            currentGrab.transform.localPosition = Vector3.zero;
        }*/
    }

    void Release()
    {
        if (currentGrab == null) return;
        if (currentGrab.GetComponent<IOutput>() != null)
            currentGrab.GetComponent<IOutput>().OutputInitialize();
        if (currentGrab.GetComponent<IInput>() != null)
            currentGrab.GetComponent<IInput>().InputInitialize();
        if (currentGrab.GetComponent<IInteractable>() != null)
            currentGrab.GetComponent<IInteractable>().InteractInitialize();
        if (currentGrab.GetComponent<Motor>() != null)
            currentGrab.GetComponent<Motor>().MotorInitialize();
        if (currentGrab.GetComponent<Bearing>() != null)
            currentGrab.GetComponent<Bearing>().BearingInitialize();
        currentGrab = null;
    }
}