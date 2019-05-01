using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicLight : MonoBehaviour, IInput
{
    public List<IOutput> ConnectedOutputs { get { return connectedOutputs; } set { connectedOutputs = value; } }
    public List<IOutput> connectedOutputs = new List<IOutput>();

    public List<Transform> InputTransforms { get { return inputTransforms; } set { inputTransforms = value; } }
    public List<Transform> inputTransforms = new List<Transform>();

    public void InputInitialize() { LogicStorage.inputs.Add(this); }

    public void ToggleInputVisibility(bool state) { foreach (var index in inputTransforms) { index.gameObject.SetActive(state); } }

    public Light light;

    public Material on;
    public Material off;

    public void OnSignal()
    {
        bool isOn = false;
        foreach (var index in connectedOutputs)
        {
            if (index.SignalState)
            {
                isOn = true;
                break;
            }
        }

        Material[] mats = GetComponent<MeshRenderer>().sharedMaterials;
        mats[1] = isOn ? on : off;
        GetComponent<MeshRenderer>().sharedMaterials = mats;
    }
}
