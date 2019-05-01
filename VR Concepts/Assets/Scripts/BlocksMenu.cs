using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksMenu : MonoBehaviour
{
    private bool isOpen = false;
    private Vector3 initialScale;
    BuildingBlocks.EBuildingBlockCategory currentCategory = BuildingBlocks.EBuildingBlockCategory.Blocks;

    private List<GameObject> currentBlocks = new List<GameObject>();

    [SerializeField] private GameObject menuTab;
    [SerializeField] int rowCount = 4;
    [SerializeField] int columnCount = 4;

    [SerializeField] private float menuTransitionTime;

    void Start()
    {
        initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
        for (int i = 0; i < Enum.GetNames(typeof(BuildingBlocks.EBuildingBlockCategory)).Length; i++)
        {
            var index = Enum.GetNames(typeof(BuildingBlocks.EBuildingBlockCategory))[i];
            if (index != "NONE")
            {
                GameObject temp = Instantiate(menuTab, transform);
                temp.transform.localPosition = new Vector3(7, 0, 4 - 2 * (i-1));
                MenuTab mt = temp.GetComponent<MenuTab>();
                mt.text.text = index;
                mt.category = (BuildingBlocks.EBuildingBlockCategory) i;
            }
        }
        SetCategory(currentCategory);
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

    public void SetBlocks(BuildingBlocks.EBuildingBlockCategory category)
    {
        int x = 0;
        int y = 0;
        foreach (var index in BuildingBlocks.manager.blocks)
        {
            if (index.category == category)
            {
                GameObject temp = Instantiate(index.gameObject, transform);
                Vector3 clampedPosition = new Vector3((float)x / columnCount, 0, (float)y / rowCount);
                temp.transform.localPosition = new Vector3(1,0,1) * -5 + clampedPosition*10 + Vector3.Scale(new Vector3(1,0,1), new Vector3(((10f/columnCount)/2f), 0, (10f / rowCount) / 2f));
                temp.transform.localScale = Vector3.one * (15f / columnCount);
                currentBlocks.Add(temp);
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

    public void SetCategory(BuildingBlocks.EBuildingBlockCategory category)
    {
        Debug.Log("Setting category to "+category);

        foreach (var index in currentBlocks)
        {
            Destroy(index);
        }

        SetBlocks(category);
    }
}