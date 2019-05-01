using UnityEngine;
using System.Collections.Generic;

public class ConnectionPoint : Item {

    public List<GameObject> connections = new List<GameObject>();

    public void AddConnection(GameObject other)
    {
        connections.Add(other);
    }

    public void RemoveConnection(GameObject other)
    {
        connections.Remove(other);
        for (int i = 0;i < GetComponents<HingeJoint2D>().Length;i++)
        {
            if (GetComponents<HingeJoint2D>()[i].connectedBody == other)
            {
                Destroy(GetComponents<HingeJoint2D>()[i]);
            }
        }

        if (gameObject.tag == "ConnectionPoint" && connections.Count <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int GetConnectionCount()
    {
        return connections.Count;
    }
}