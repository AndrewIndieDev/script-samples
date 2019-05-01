using UnityEngine;
using System.Collections.Generic;

public class ConnectionLine : MonoBehaviour {

    public ConnectionPoint[] connections = new ConnectionPoint[2];
    public HingeJoint2D[] hingeJoints = new HingeJoint2D[2];
    public int id;

    public void GetID()
    {
        id = IDManagement.GetUniqueID();
    }

    public List<int> GetConnectedIDs()
    {
        List<int> list = new List<int>();

        foreach (ConnectionPoint obj in connections)
        {
            if (obj.GetComponent<ConnectionPoint>() != null)
                list.Add(obj.GetComponent<ConnectionPoint>().id);
        }
        return list;
    }

    public void DestroyLine()
    {
        connections[0].RemoveConnection(gameObject);
        connections[1].RemoveConnection(gameObject);
        Destroy(hingeJoints[0]);
        Destroy(hingeJoints[1]);
        Destroy(gameObject);
    }

    public void UpdateLine()
    {
        transform.position = Vector3.Lerp(connections[0].transform.position, connections[1].transform.position, 0.5f);
        transform.localScale = new Vector3(Vector3.Distance(connections[1].transform.position, connections[0].transform.position), transform.localScale.y, transform.localScale.z);
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(connections[0].transform.position.y - connections[1].transform.position.y, connections[0].transform.position.x - connections[1].transform.position.x) * Mathf.Rad2Deg, Vector3.forward);
    }
}
