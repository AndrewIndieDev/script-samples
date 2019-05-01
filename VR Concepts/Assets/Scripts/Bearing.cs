using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bearing : MonoBehaviour
{

    public GameObject input;

    public void Rotate(float speed)
    {
        transform.rotation *= Quaternion.AngleAxis(speed, transform.forward);
    }

    public void ToggleInputs(bool toggle)
    {
        if (input == null) return;
        input.SetActive(toggle);
    }

    public void BearingInitialize()
    {
        LogicStorage.bearingInputs.Add(input);
    }
}
