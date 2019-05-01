using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialsMenu : MonoBehaviour
{
    private bool isOpen = false;
    private Vector3 initialScale;
    [SerializeField] private LayerMask blockLayer;

    [SerializeField] int rowCount = 4;
    [SerializeField] int columnCount = 4;

    [SerializeField] private float menuTransitionTime;

    void Start()
    {
        initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
        SetMaterials();
    }

    void Update()
    {
        bool lookingAtMenu = Vector3.Dot(transform.up, Camera.main.transform.forward) < -0.75f;
        if (!isOpen && lookingAtMenu)
        {
            isOpen = true;
            OpenMenu();
        }
        if (isOpen && !lookingAtMenu)
        {
            isOpen = false;
            CloseMenu();
        }
    }

    void OpenMenu()
    {
        StopAllCoroutines();
        StartCoroutine(OpenAnimation());
    }

    IEnumerator OpenAnimation()
    {
        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime / menuTransitionTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, i);
            yield return 0;
        }
    }

    void CloseMenu()
    {
        StopAllCoroutines();
        StartCoroutine(CloseAnimation());
    }

    IEnumerator CloseAnimation()
    {
        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime / menuTransitionTime;
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, i);
            yield return 0;
        }
    }

    public void SetMaterials()
    {
        int x = 0;
        int y = 0;
        foreach (var index in BuildingBlocks.manager.presetMaterials)
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            temp.transform.parent = transform;
            Vector3 clampedPosition = new Vector3((float)x / columnCount, 0, (float)y / rowCount);
            temp.transform.localPosition = new Vector3(1, 0, 1) * -5 + clampedPosition * 10 + Vector3.Scale(new Vector3(1, 0, 1), new Vector3(((10f / columnCount) / 2f), 0, (10f / rowCount) / 2f));
            temp.transform.localScale = Vector3.one * (7.5f / columnCount);
            temp.transform.gameObject.GetComponent<MeshRenderer>().sharedMaterial = index;
            temp.gameObject.layer = LayerMask.NameToLayer("MenuItem");
            ++x;
            if (x >= columnCount)
            {
                ++y;
                x = 0;
            }
            if (y >= rowCount)
            {
                break;
            }
        }
    }
}