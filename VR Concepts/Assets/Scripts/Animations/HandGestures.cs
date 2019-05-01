using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGestures : MonoBehaviour
{
    public GameObject rightController;
    public GameObject leftController;

    public List<GameObject> gestures = new List<GameObject>();
    public int currentGesture = 0;
    private Vector2 pos;

    public static HandGestures manager;

	void Start ()
    {
        manager = this;
        VRInputManager.inputEventTouchpadRDown += ChangeGesture;
	}

    void OnDestroy()
    {
        VRInputManager.inputEventTouchpadRDown -= ChangeGesture;
    }

    void ChangeGesture()
    {
        pos = UpdatePos();

        if (pos.x > 0)
        {
            Destroy(rightController.GetComponentInChildren<MeshRenderer>().gameObject);
            GameObject obj = Instantiate(gestures[1], transform);
            obj.transform.SetParent(rightController.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            currentGesture = 1;
        }
        else if (pos.x < 0)
        {
            Destroy(rightController.GetComponentInChildren<MeshRenderer>().gameObject);
            GameObject obj = Instantiate(gestures[0], transform);
            obj.transform.SetParent(rightController.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            currentGesture = 0;
        }
    }

    Vector2 UpdatePos()
    {
        //return VRInputManager.GetTouchPosR();
        //return SteamVR_Controller.Input((int)trackedObjR.index).GetAxis();
        return Vector2.zero;
    }
}
