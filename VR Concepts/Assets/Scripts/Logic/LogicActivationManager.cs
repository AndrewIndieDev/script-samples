using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicActivationManager : MonoBehaviour
{
    public LayerMask blocksLayer;
    IInteractable interactibleObject;
    public Transform cast;
    public float radius = 0.05f;

    private void Start()
    {
        VRInputManager.inputEventTriggerRDown += Interact;
        VRInputManager.inputEventTriggerRUp += Part;
    }

    private void OnDestroy()
    {
        VRInputManager.inputEventTriggerRDown -= Interact;
        VRInputManager.inputEventTriggerRUp -= Part;
    }

    void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(cast.position, radius, blocksLayer);
        GameObject closest = null;

        float closestDist = Mathf.Infinity;

        foreach (var index in colliders)
        {
            float temp = Vector3.Distance(index.transform.position, cast.position);
            if (temp < closestDist && index.gameObject.CompareTag("Interactable"))
            {
                closestDist = temp;
                closest = index.gameObject;
            }
        }
        if (closest == null) return;

        interactibleObject = closest.GetComponent<IInteractable>();

        if (interactibleObject != null)
            interactibleObject.OnInteract();
    }

    void Part()
    {
        if (interactibleObject == null) return;

        interactibleObject.OnPart();
        interactibleObject = null;
    }
}
