using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ORGate : MonoBehaviour, IInput, IOutput {
    
    public List<IOutput> ConnectedOutputs { get { return connectedOutputs; } set { connectedOutputs = value; } }
    [SerializeField] private List<IOutput> connectedOutputs = new List<IOutput>();

    public List<IInput> ConnectedInputs { get { return connectedInputs; } set { connectedInputs = value; } }
    [SerializeField] private List<IInput> connectedInputs = new List<IInput>();

    public List<Transform> InputTransforms { get { return inputTransforms; } set { inputTransforms = value; } }
    [SerializeField] private List<Transform> inputTransforms = new List<Transform>();

    public List<Transform> OutputTransforms { get { return outputTransforms; } set { outputTransforms = value; } }
    [SerializeField] private List<Transform> outputTransforms = new List<Transform>();

    public void ToggleInputVisibility(bool state) { foreach (var index in inputTransforms) { index.gameObject.SetActive(state); } }
    public void ToggleOutputVisibility(bool state) { foreach (var index in outputTransforms) { index.gameObject.SetActive(state); } }

    public void InputInitialize() { LogicStorage.inputs.Add(this); }
    public void OutputInitialize() { LogicStorage.outputs.Add(this); }

    public bool SignalState { get { return signalState; } set { signalState = value; } }
    [SerializeField] private bool signalState = false;

    public void OnSignal()
    {
        bool oneIsOn = false;
        foreach (var index in ConnectedOutputs)
        {
            if (index.SignalState == true)
            {
                oneIsOn = true;
                break;
            }
        }
        if (oneIsOn != SignalState)
        {
            SignalState = oneIsOn;
            foreach (var index in ConnectedInputs)
            {
                index.OnSignal();
            }
        }
    }
}
