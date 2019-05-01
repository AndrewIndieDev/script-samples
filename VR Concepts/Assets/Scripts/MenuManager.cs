using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public enum EMenuType
    {
        Blocks,
        Materials,
        Wire,
        Remove,



        None
    }

    public List<GameObject> menus = new List<GameObject>();
    public static MenuManager manager;

    void Start()
    {
        manager = this;
    }

    public void SetMenu(EMenuType type)
    {
        if (type != EMenuType.None)
        {
            foreach (GameObject index in menus)
            {
                if (index != null)
                    index.SetActive(false);
            }
            if (menus[(int)type] != null)
                menus[(int)type].SetActive(true);
        }
    }
}
