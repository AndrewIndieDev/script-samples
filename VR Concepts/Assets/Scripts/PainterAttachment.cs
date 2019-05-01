using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainterAttachment : MonoBehaviour
{
    public LayerMask menuBlockLayer;
    public LayerMask blocks;
    public Transform sphereCastObjectTransform;
    public float raycastRadius = 0.05f;
    public ParticleSystem ps;
    private Material currentMaterial;

	// Use this for initialization
	void Start ()
	{
	    currentMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterials[1];

        VRInputManager.inputEventTriggerRDown += Pick;
	    MenuManager.manager.SetMenu(MenuManager.EMenuType.Materials);
    }

    void OnDestroy()
    {
        VRInputManager.inputEventTriggerRDown -= Pick;
    }

    void Pick()
    {
        Collider[] menuMaterials = Physics.OverlapSphere(sphereCastObjectTransform.position, raycastRadius, menuBlockLayer);
        GameObject closest = null;
        float closestDist = Mathf.Infinity;
        foreach (var index in menuMaterials)
        {
            float dist = Vector3.Distance(index.transform.position, sphereCastObjectTransform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = index.transform.gameObject;
            }
        }
        if (closest != null)
        {
            MeshRenderer thisOne = gameObject.GetComponent<MeshRenderer>();
            MeshRenderer otherOne = closest.GetComponent<MeshRenderer>();
            Material[] mats = thisOne.sharedMaterials;
            mats[1] = otherOne.sharedMaterial;
            currentMaterial = mats[1];
            thisOne.sharedMaterials = mats;
            var main = ps.main;
            main.startColor = mats[1].color;
        }
        else
        {
            Paint();
        }
    }

    void Paint()
    {
        ps.Play();
        RaycastHit hit;
        if (Physics.Raycast(sphereCastObjectTransform.position, sphereCastObjectTransform.forward, out hit, 0.2f, blocks))
        {
            hit.transform.gameObject.GetComponent<MeshRenderer>().sharedMaterial = currentMaterial;
        }
    }
}