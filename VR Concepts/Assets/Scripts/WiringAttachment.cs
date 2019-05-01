using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiringAttachment : MonoBehaviour
{
    enum EWiringMode
    {
        LOGIC,
        MOTOR
    }

    public LayerMask wiringLayer;
    public Transform sphereCastObjectTransform;
    public float raycastRadius = 0.05f;
    private GameObject grabbedObject;
    private GameObject targetObject;
    private IInput targetObjectInput;
    private Bearing targetBearingInput;
    private IOutput grabbedObjectOutput;
    private Motor grabbedObjectMotor;
    private LineRenderer lr;
    private EWiringMode wiringMode;

    void Start()
    {
        VRInputManager.inputEventTriggerRDown += Grab;
        VRInputManager.inputEventTriggerR += UpdateGrab;
        VRInputManager.inputEventTriggerRUp += ReleaseGrab;
        ToggleVisibility(EVisibility.Outputs);
        MenuManager.manager.SetMenu(MenuManager.EMenuType.Wire);
    }

    void OnDestroy()
    {
        VRInputManager.inputEventTriggerRDown -= Grab;
        VRInputManager.inputEventTriggerR -= UpdateGrab;
        VRInputManager.inputEventTriggerRUp -= ReleaseGrab;
        ToggleVisibility(EVisibility.None);
        if (lr != null)
        {
            Destroy(lr.gameObject);
        }
    }

    void Grab()
    {
        Collider[] wiringSpots = Physics.OverlapSphere(sphereCastObjectTransform.position, raycastRadius, wiringLayer);
        GameObject closest = null;
        float closestDist = Mathf.Infinity;
        foreach (var index in wiringSpots)
        {
            float dist = Vector3.Distance(index.transform.position, sphereCastObjectTransform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = index.transform.gameObject;
            }
        }
        if (closest == null) return;

        GameObject wire = new GameObject("Wire");
        lr = wire.AddComponent<LineRenderer>();
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.SetPosition(0, closest.transform.position);
        grabbedObject = closest;
        if (closest.transform.parent.GetComponent<IOutput>() != null)
        {
            grabbedObjectOutput = closest.transform.parent.GetComponent<IOutput>();
            wiringMode = EWiringMode.LOGIC;
        }
        else if (closest.transform.parent.GetComponent<Motor>() != null)
        {
            grabbedObjectMotor = closest.transform.parent.GetComponent<Motor>();
            wiringMode = EWiringMode.MOTOR;
        }
        ToggleVisibility((wiringMode == EWiringMode.LOGIC) ? EVisibility.Inputs : EVisibility.BearingInputs);
    }

    void UpdateGrab()
    {
        if (grabbedObject == null) return;

        Collider[] wiringSpots = Physics.OverlapSphere(sphereCastObjectTransform.position, raycastRadius, wiringLayer);
        GameObject closest = null;
        float closestDist = Mathf.Infinity;
        foreach (var index in wiringSpots)
        {
            float dist = Vector3.Distance(index.transform.position, sphereCastObjectTransform.position);
            if (dist < closestDist && ((index.transform.parent.GetComponent<IInput>() != null && wiringMode == EWiringMode.LOGIC) || (index.transform.parent.GetComponent<Bearing>() != null && wiringMode == EWiringMode.MOTOR)))
            {
                closestDist = dist;
                closest = index.transform.gameObject;
            }
        }
        if (closest == null)
        {
            lr.SetPosition(1, sphereCastObjectTransform.position);
        }
        else
        {
            if (wiringMode == EWiringMode.LOGIC)
            {
                lr.SetPosition(1, closest.transform.parent.GetComponent<IInput>().InputTransforms[0].position);
                targetObject = closest;
                targetObjectInput = closest.transform.parent.GetComponent<IInput>();
            }
            else if (wiringMode == EWiringMode.MOTOR)
            {
                lr.SetPosition(1, closest.transform.parent.position);
                targetObject = closest;
                targetBearingInput = closest.transform.parent.GetComponent<Bearing>();
            }
        }
    }

    void ReleaseGrab()
    {
        ToggleVisibility(EVisibility.Outputs);

        if (wiringMode == EWiringMode.LOGIC)
        {
            bool deleteLR = true;
            if (!(grabbedObject == null || grabbedObjectOutput == null || targetObject == null ||
                  targetObjectInput == null))
            {
                lr.SetPosition(0, grabbedObject.transform.position);
                lr.SetPosition(1, targetObject.transform.position);
                if (!grabbedObjectOutput.ConnectedInputs.Contains(targetObjectInput))
                {
                    if (grabbedObject.transform.parent != targetObject.transform.parent)
                    {
                        grabbedObjectOutput.ConnectedInputs.Add(targetObjectInput);
                        targetObjectInput.ConnectedOutputs.Add(grabbedObjectOutput);
                        if (grabbedObject.transform.parent.GetComponent<IInput>() != null)
                        {
                            grabbedObject.transform.parent.GetComponent<IInput>().OnSignal();
                        }

                        deleteLR = false;
                    }
                }
            }
            if (deleteLR)
            {
                if (lr != null)
                    Destroy(lr.gameObject);
            }
        }
        else if (wiringMode == EWiringMode.MOTOR)
        {
            bool deleteLR = true;
            if (!(grabbedObject == null || grabbedObjectMotor == null || targetObject == null ||
                  targetBearingInput == null))
            {
                lr.SetPosition(0, grabbedObject.transform.position);
                lr.SetPosition(1, targetObject.transform.position);
                bool exists = false;

                foreach (var index in grabbedObjectMotor.bearings)
                {
                    if (index.bearing == targetBearingInput)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    grabbedObjectMotor.bearings.Add(new Motor.ConnectedBearing(targetBearingInput));
                    deleteLR = false;
                }
            }
            if (deleteLR)
            {
                if (lr != null)
                    Destroy(lr.gameObject);
            }
        }

        grabbedObject = null;
        grabbedObjectOutput = null;
        targetObject = null;
        targetObjectInput = null;
        lr = null;

    }

    enum EVisibility
    {
        Inputs,
        Outputs,
        BearingInputs,
        None
    }

    void ToggleVisibility(EVisibility visibility)
    {
        foreach (var index in LogicStorage.outputs)
        {
            index.ToggleOutputVisibility(visibility == EVisibility.Outputs);
        }
        foreach (var index in LogicStorage.inputs)
        {
            index.ToggleInputVisibility(visibility == EVisibility.Inputs);
        }
        foreach (var index in LogicStorage.motorOutputs)
        {
            index.SetActive(visibility == EVisibility.Outputs);
        }
        foreach (var index in LogicStorage.bearingInputs)
        {
            index.SetActive(visibility == EVisibility.BearingInputs);
        }
    }
}
