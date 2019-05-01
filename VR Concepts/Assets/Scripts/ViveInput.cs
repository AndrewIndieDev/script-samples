using UnityEngine;
using Valve.VR;

public class ViveInput : MonoBehaviour
{
    public SteamVR_ActionSet p_ActionSet;
    public SteamVR_Action_Boolean p_BooleanAction;
    public SteamVR_Action_Vector2 p_TouchPosition;

    private void Awake()
    {
        //events
        p_BooleanAction[SteamVR_Input_Sources.LeftHand].onStateDown += BoolTest;
        p_BooleanAction[SteamVR_Input_Sources.RightHand].onStateDown += BoolTest;
        p_TouchPosition[SteamVR_Input_Sources.Any].onAxis += AxisTest;
    }

    private void OnDestroy()
    {
        //events
        p_BooleanAction[SteamVR_Input_Sources.LeftHand].onStateDown -= BoolTest;
        p_BooleanAction[SteamVR_Input_Sources.RightHand].onStateDown -= BoolTest;
        p_TouchPosition[SteamVR_Input_Sources.Any].onAxis -= AxisTest;
    }

    private void Start()
    {
        //p_ActionSet.Activate(SteamVR_Input_Sources.Any, 0, true);
    }

    private void BoolTest(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
        Debug.Log(source.ToString() + " | " + action.state);
    }

    private void AxisTest(SteamVR_Action_Vector2 action, SteamVR_Input_Sources source, Vector2 axis, Vector2 delta)
    {
        Debug.Log(source.ToString() + " | " + axis + " | " + delta);
    }
}
