using System.Collections.Generic;
using UnityEngine;

public interface IInput
{
    List<IOutput> ConnectedOutputs { get; set; }
    List<Transform> InputTransforms { get; set; }
    void OnSignal();
    void ToggleInputVisibility(bool state);
    void InputInitialize();
}

public interface IOutput
{
    List<IInput> ConnectedInputs { get; set; }
    List<Transform> OutputTransforms { get; set; }
    bool SignalState { get; set; }
    void ToggleOutputVisibility(bool state);
    void OutputInitialize();
}

public interface IInteractable
{
    void OnInteract();
    void OnPart();
    void InteractInitialize();
}