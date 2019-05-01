using UnityEngine;
using System.Collections.Generic;

public class Item : MonoBehaviour {

    public Vector3 originalScale;
    bool hasSetSize;
    public int id;
    public EBuildType buildType;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void GetID()
    {
        id = IDManagement.GetUniqueID();
    }

    public List<int> GetConnectedIDs()
    {
        if (GetComponent<ConnectionPoint>() != null)
        {
            List<int> list = new List<int>();

            foreach (GameObject obj in GetComponent<ConnectionPoint>().connections)
            {
                if (obj.GetComponent<ConnectionLine>() != null)
                    list.Add(obj.GetComponent<ConnectionLine>().id);
            }
            return list;
        }
        else
        {
            return new List<int>();
        }
    }

    public virtual void OnActivate()
    {

    }

    // ON MOUSE OVER GLOW //
    #region
    void OnMouseOver()
    {
        if (hasSetSize == false)
        {
            originalScale = transform.localScale;
            hasSetSize = true;
        }

        if (!Control.manager.gameActive)
        {
            transform.localScale = originalScale * 1.2f;
        }
    }

    void OnMouseExit()
    {
        if (!Control.manager.gameActive)
        {
            transform.localScale = originalScale;
        }
    }
    #endregion
}
