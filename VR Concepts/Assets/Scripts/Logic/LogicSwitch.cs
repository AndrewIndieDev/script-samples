using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicSwitch : MonoBehaviour, IOutput, IInteractable {

    public List<IInput> ConnectedInputs { get { return connectedInputs; } set { connectedInputs = value; } }
    [SerializeField] private List<IInput> connectedInputs = new List<IInput>();

    public List<Transform> OutputTransforms { get { return outputTransforms; } set { outputTransforms = value; } }
    [SerializeField] private List<Transform> outputTransforms = new List<Transform>();
    
    public void ToggleOutputVisibility(bool state) { foreach (var index in outputTransforms) { index.gameObject.SetActive(state); } }

    public void OutputInitialize() { LogicStorage.outputs.Add(this); }
    public void InteractInitialize() { LogicStorage.interactables.Add(this); }

    public bool SignalState { get { return signalState; } set { signalState = value; } }
    [SerializeField] private bool signalState = false;

    private bool on = false;

    void OnClick(bool state)
    {
        if (state == signalState) return;

        signalState = state;
        foreach (var index in ConnectedInputs)
        {
            index.OnSignal();
        }
    }

    public void OnInteract()
    {
        OnClick(!on);
        on = !on;
    }

    public void OnPart()
    {
        //Nothing
    }
}
