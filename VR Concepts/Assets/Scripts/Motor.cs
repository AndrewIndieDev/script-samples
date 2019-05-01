using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Motor : MonoBehaviour, IInput
{
    public class ConnectedBearing
    {
        public Bearing bearing;
        public float speed;

        public ConnectedBearing(Bearing b, float s = 5)
        {
            bearing = b;
            speed = s;
        }
    }

    public List<IOutput> ConnectedOutputs {get { return connectedoutputs; } set { connectedoutputs = value; }}
    [SerializeField] private List<IOutput> connectedoutputs = new List<IOutput>();
    public List<Transform> InputTransforms {get { return inputTransforms; } set { inputTransforms = value; }}
    [SerializeField] private List<Transform> inputTransforms = new List<Transform>();
    public void ToggleInputVisibility(bool state) { foreach (var index in inputTransforms) { index.gameObject.SetActive(state); } }
    public void InputInitialize() { LogicStorage.inputs.Add(this); }
    private bool isOn = false;
    public GameObject output;
    public List<ConnectedBearing> bearings = new List<ConnectedBearing>();

    public void OnSignal()
    {
        isOn = false;

        foreach (var index in connectedoutputs)
        {
            if (index.SignalState)
            {
                isOn = true;
                return;
            }
        }
    }

    void Update()
    {
        if (isOn)
        {
            foreach (var index in bearings)
            {
                index.bearing.Rotate(index.speed);
            }
        }
    }

    public void ToggleOutputs(bool toggle)
    {
        if (output == null) return;
        output.SetActive(toggle);
    }

    public void MotorInitialize()
    {
        LogicStorage.motorOutputs.Add(output);
    }
}
