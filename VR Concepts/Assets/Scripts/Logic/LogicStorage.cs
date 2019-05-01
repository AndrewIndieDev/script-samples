using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicStorage : MonoBehaviour {

    public static List<IOutput> outputs = new List<IOutput>();
    public static List<IInput> inputs = new List<IInput>();
    public static List<IInteractable> interactables = new List<IInteractable>();
    public static List<GameObject> motorOutputs = new List<GameObject>();
    public static List<GameObject> bearingInputs = new List<GameObject>();
}
