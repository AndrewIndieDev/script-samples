using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    //public SteamVR_ControllerManager cm;\

    private GameObject pickedUpObject = null;

    // Use this for initialization
    void Start()
    {
        VRInputManager.inputEventTriggerLDown += PickupObject;
        VRInputManager.inputEventTriggerLUp += DropObject;
    }

    void OnDestroy()
    {
        VRInputManager.inputEventTriggerLDown -= PickupObject;
        VRInputManager.inputEventTriggerLUp -= DropObject;
    }

    void PickupObject()
    {
        Debug.Log("Trigger Down");
        //Collider[] cols = Physics.OverlapSphere(cm.left.transform.position, 0.2f);
        //foreach (var col in cols)
        //{
        //    if (col.tag == "Pickuppable")
        //    {
        //        //col.transform.parent = cm.left.transform;
        //        pickedUpObject = col.gameObject;
        //        pickedUpObject.GetComponent<Rigidbody>().isKinematic = true;
        //    }
        //}
    }

    void DropObject()
    {
        Debug.Log("Trigger Up");
        if (pickedUpObject != null)
        {
            pickedUpObject.transform.parent = null;
            pickedUpObject.GetComponent<Rigidbody>().isKinematic = false;
            //pickedUpObject.GetComponent<Rigidbody>().velocity = VRInputManager.GetVelocityL();
            //pickedUpObject.GetComponent<Rigidbody>().angularVelocity = VRInputManager.GetAngularVelocityL();
            pickedUpObject = null;
        }
    }
}