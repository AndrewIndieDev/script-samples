using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerComponentDisabler : MonoBehaviour //NetworkBehaviour
{

    /*public Behaviour[] disableComponents;
    public GameObject[] enableGameObjects;
    public Behaviour[] enableComponents;

    void Start()
    {
        if (!isLocalPlayer)
        {
            foreach (var index in disableComponents)
            {
                index.enabled = false;
            }
            foreach (var index in enableComponents)
            {
                index.enabled = true;
            }
            foreach (var index in enableGameObjects)
            {
                index.SetActive(true);
            }
        }
        else
        {
            VRInputManager.manager.leftController = GetComponent<SteamVR_ControllerManager>().left;
            VRInputManager.manager.rightController = GetComponent<SteamVR_ControllerManager>().right;
            VRInputManager.manager.Awake();
        }
    }
    */
}
