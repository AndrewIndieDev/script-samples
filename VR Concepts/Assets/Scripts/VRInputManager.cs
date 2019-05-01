using UnityEngine;
using System.Collections;
using Valve.VR;

public class VRInputManager : MonoBehaviour
{
    
    //Trigger
    public delegate void DelegateInputTriggerL();
    public delegate void DelegateInputTriggerLDown();
    public delegate void DelegateInputTriggerLUp();
    public delegate void DelegateInputTriggerR();
    public delegate void DelegateInputTriggerRDown();
    public delegate void DelegateInputTriggerRUp();
    //Menu
    public delegate void DelegateInputMenuL();
    public delegate void DelegateInputMenuLDown();
    public delegate void DelegateInputMenuLUp();
    public delegate void DelegateInputMenuR();
    public delegate void DelegateInputMenuRDown();
    public delegate void DelegateInputMenuRUp();
    //Grip
    public delegate void DelegateInputGripL();
    public delegate void DelegateInputGripLDown();
    public delegate void DelegateInputGripLUp();
    public delegate void DelegateInputGripR();
    public delegate void DelegateInputGripRDown();
    public delegate void DelegateInputGripRUp();
    //Touchpad
    public delegate void DelegateInputTouchpadL();
    public delegate void DelegateInputTouchpadLDown();
    public delegate void DelegateInputTouchpadLUp();
    public delegate void DelegateInputTouchpadR();
    public delegate void DelegateInputTouchpadRDown();
    public delegate void DelegateInputTouchpadRUp();


    //Trigger
    public static event DelegateInputTriggerL inputEventTriggerL;
    public static event DelegateInputTriggerLDown inputEventTriggerLDown;
    public static event DelegateInputTriggerLUp inputEventTriggerLUp;
    public static event DelegateInputTriggerR inputEventTriggerR;
    public static event DelegateInputTriggerRDown inputEventTriggerRDown;
    public static event DelegateInputTriggerRUp inputEventTriggerRUp;
    //Menu
    public static event DelegateInputMenuL inputEventMenuL;
    public static event DelegateInputMenuLDown inputEventMenuLDown;
    public static event DelegateInputMenuLUp inputEventMenuLUp;
    public static event DelegateInputMenuR inputEventMenuR;
    public static event DelegateInputMenuRDown inputEventMenuRDown;
    public static event DelegateInputMenuRUp inputEventMenuRUp;
    //Grip
    public static event DelegateInputGripL inputEventGripL;
    public static event DelegateInputGripLDown inputEventGripLDown;
    public static event DelegateInputGripLUp inputEventGripLUp;
    public static event DelegateInputGripR inputEventGripR;
    public static event DelegateInputGripRDown inputEventGripRDown;
    public static event DelegateInputGripRUp inputEventGripRUp;
    //Touchpad
    public static event DelegateInputTouchpadL inputEventTouchpadL;
    public static event DelegateInputTouchpadLDown inputEventTouchpadLDown;
    public static event DelegateInputTouchpadLUp inputEventTouchpadLUp;
    public static event DelegateInputTouchpadR inputEventTouchpadR;
    public static event DelegateInputTouchpadRDown inputEventTouchpadRDown;
    public static event DelegateInputTouchpadRUp inputEventTouchpadRUp;

    static SteamVR_TrackedObject trackedObjR;
    static SteamVR_TrackedObject trackedObjL;
    public GameObject leftController;
    public GameObject rightController;

    public static VRInputManager manager;

    public void Awake()
    {
        if (leftController != null && rightController != null)
        {
            trackedObjR = rightController.GetComponent<SteamVR_TrackedObject>();
            trackedObjL = leftController.GetComponent<SteamVR_TrackedObject>();
        }
    }

    // Use this for initialization
    void Start () {
        manager = this;
    }
	/*
	// Update is called once per frame
	void Update ()
	{
	    if (trackedObjR == null || trackedObjL == null) return;

	    if (trackedObjR.gameObject.activeInHierarchy == true)
	    {
	        //Right Controller
	        var deviceR = SteamVR_Controller.Input((int) trackedObjR.index);

            //Trigger
            if (deviceR.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && inputEventTriggerRDown != null)
                inputEventTriggerRDown();
            if (deviceR.GetPress(SteamVR_Controller.ButtonMask.Trigger) && inputEventTriggerR != null)
                inputEventTriggerR();
            if (deviceR.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && inputEventTriggerRUp != null)
                inputEventTriggerRUp();

            //Menu
            if (deviceR.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputEventMenuRDown != null)
                inputEventMenuRDown();
	        if (deviceR.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputEventMenuR != null)
	            inputEventMenuR();
            if (deviceR.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputEventMenuRUp != null)
                inputEventMenuRUp();

            //Grip
            if (deviceR.GetPressDown(SteamVR_Controller.ButtonMask.Grip) && inputEventGripRDown != null)
                inputEventGripRDown();
            if (deviceR.GetPress(SteamVR_Controller.ButtonMask.Grip) && inputEventGripR != null)
                inputEventGripR();
            if (deviceR.GetPressUp(SteamVR_Controller.ButtonMask.Grip) && inputEventGripRUp != null)
                inputEventGripRUp();

            //Touchpad
            if (deviceR.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && inputEventTouchpadRDown != null)
                inputEventTouchpadRDown();
            if (deviceR.GetPress(SteamVR_Controller.ButtonMask.Touchpad) && inputEventTouchpadR != null)
                inputEventTouchpadR();
            if (deviceR.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && inputEventTouchpadRUp != null)
                inputEventTouchpadRUp();
        }

	    if (trackedObjL.gameObject.activeInHierarchy == true)
	    {
	        //Left Controller
	        var deviceL = SteamVR_Controller.Input((int) trackedObjL.index);

            //Trigger
            if (deviceL.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && inputEventTriggerLDown != null)
                inputEventTriggerLDown();
            if (deviceL.GetPress(SteamVR_Controller.ButtonMask.Trigger) && inputEventTriggerL != null)
                inputEventTriggerL();
            if (deviceL.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && inputEventTriggerLUp != null)
                inputEventTriggerLUp();

            //Menu
            if (deviceL.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputEventMenuLDown != null)
                inputEventMenuLDown();
            if (deviceL.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputEventMenuL != null)
                inputEventMenuL();
            if (deviceL.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputEventMenuLUp != null)
                inputEventMenuLUp();

            //Grip
            if (deviceL.GetPressDown(SteamVR_Controller.ButtonMask.Grip) && inputEventGripLDown != null)
                inputEventGripLDown();
            if (deviceL.GetPress(SteamVR_Controller.ButtonMask.Grip) && inputEventGripL != null)
                inputEventGripL();
            if (deviceL.GetPressUp(SteamVR_Controller.ButtonMask.Grip) && inputEventGripLUp != null)
                inputEventGripLUp();

            //Touchpad
            if (deviceL.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && inputEventTouchpadLDown != null)
                inputEventTouchpadLDown();
            if (deviceL.GetPress(SteamVR_Controller.ButtonMask.Touchpad) && inputEventTouchpadL != null)
                inputEventTouchpadL();
            if (deviceL.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && inputEventTouchpadLUp != null)
                inputEventTouchpadLUp();
        }
	}

    public static Vector3 GetVelocityL()
    {
        var deviceL = SteamVR_Controller.Input((int)trackedObjL.index);
        return deviceL.velocity;
    }

    public static Vector3 GetVelocityR()
    {
        var deviceR = SteamVR_Controller.Input((int)trackedObjR.index);
        return deviceR.velocity;
    }

    public static Vector3 GetAngularVelocityL()
    {
        var deviceL = SteamVR_Controller.Input((int)trackedObjL.index);
        return deviceL.angularVelocity;
    }

    public static Vector3 GetAngularVelocityR()
    {
        var deviceR = SteamVR_Controller.Input((int)trackedObjR.index);
        return deviceR.angularVelocity;
    }

    public static Vector2 GetTouchPosL()
    {
        var deviceL = SteamVR_Controller.Input((int)trackedObjL.index);
        return deviceL.GetAxis();
    }

    public static Vector2 GetTouchPosR()
    {
        var deviceR = SteamVR_Controller.Input((int)trackedObjR.index);
        return deviceR.GetAxis();
    }

    public static IEnumerator VibrationL(float length, float strength, bool fadeout = false)
    {
        var deviceL = SteamVR_Controller.Input((int)trackedObjL.index);
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            deviceL.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, fadeout ? (length - i) * strength : strength));
            yield return null;
        }
    }

    public static IEnumerator VibrationR(float length, float strength, bool fadeout = false)
    {
        var deviceR = SteamVR_Controller.Input((int)trackedObjR.index);
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            deviceR.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, fadeout ? (length - i) * strength : strength));
            yield return null;
        }
    }
    */
}
