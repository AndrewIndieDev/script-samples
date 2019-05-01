using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveStateManager : MonoBehaviour
{

    [Serializable]
    public class ItemInstance
    {
        public int id;
        public float x;
        public float y;
        public float z;
        public float rotX;
        public float rotY;
        public float rotZ;
        public float rotW;
        public float scaleX;
        public float scaleY;
        public float scaleZ;
        public EBuildType buildType;
        public List<int> connectionIDs = new List<int>();
    }

	public static void SaveState()
    {
        List<ItemInstance> instances = new List<ItemInstance>();
        Item[] allItems = FindObjectsOfType<Item>();
        ConnectionLine[] allLines = FindObjectsOfType<ConnectionLine>();
        IDManagement.ResetIDs();
        String scene = SceneManager.GetActiveScene().name;

        foreach (Item item in allItems)
        {
            item.GetID();
        }
        foreach (ConnectionLine line in allLines)
        {
            line.GetID();
        }
        foreach (Item item in allItems)
        {
            if ((int)item.buildType > 1)
                instances.Add(new ItemInstance()
                {
                    id = item.id,
                    x = item.transform.position.x,
                    y = item.transform.position.y,
                    z = item.transform.position.z,
                    rotX = item.transform.rotation.x,
                    rotY = item.transform.rotation.y,
                    rotZ = item.transform.rotation.z,
                    rotW = item.transform.rotation.w,
                    scaleX = item.transform.localScale.x,
                    scaleY = item.transform.localScale.y,
                    scaleZ = item.transform.localScale.z,
                    buildType = item.buildType,
                    connectionIDs = item.GetConnectedIDs()
                });
        }
        foreach (ConnectionLine line in allLines)
        {
            instances.Add(new ItemInstance()
            {
                id = line.id,
                x = line.transform.position.x,
                y = line.transform.position.y,
                z = line.transform.position.z,
                rotX = line.transform.rotation.x,
                rotY = line.transform.rotation.y,
                rotZ = line.transform.rotation.z,
                rotW = line.transform.rotation.w,
                scaleX = line.transform.localScale.x,
                scaleY = line.transform.localScale.y,
                scaleZ = line.transform.localScale.z,
                buildType = line.gameObject.GetComponent<BoxCollider2D>().isTrigger ? EBuildType.LITELINE : EBuildType.SOLIDLINE,
                connectionIDs = line.GetConnectedIDs()
            });
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + scene + ".dat");
        bf.Serialize(file, instances);
        file.Close();
    }

    public static void LoadState()
    {
        String scene = SceneManager.GetActiveScene().name;
        if (!File.Exists(Application.persistentDataPath + "/"+scene+".dat")) { Debug.Log("File Not Found!");  return; }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/"+scene+".dat", FileMode.Open);
        List<ItemInstance> instances = (List<ItemInstance>)bf.Deserialize(file);
        file.Close();
        List<ConnectionPoint> connectionPoints = new List<ConnectionPoint>();
        List<GameObject> gameObjects = new List<GameObject>();

        foreach (ItemInstance inst in instances)
        {
            GameObject temp = Instantiate(
                Control.manager.userItems[(int)inst.buildType].buildTypePrefab,
                new Vector3(inst.x, inst.y, inst.z),
                new Quaternion(inst.rotX, inst.rotY, inst.rotZ, inst.rotW));

            temp.transform.localScale = new Vector3(inst.scaleX, inst.scaleY, inst.scaleZ);

            if (inst.buildType == EBuildType.SOLIDLINE || inst.buildType == EBuildType.LITELINE)
            {
                temp.GetComponent<ConnectionLine>().id = inst.id;
            }
            else
            {
                temp.GetComponent<Item>().id = inst.id;
            }

            connectionPoints.Add(temp.GetComponent<ConnectionPoint>());

            gameObjects.Add(temp);
        }

        for (int i = 0; i < instances.Count; ++i)
        {
            ItemInstance inst = instances[i];
            if (inst.buildType == EBuildType.SOLIDLINE || inst.buildType == EBuildType.LITELINE)
            {
                ConnectionLine line = gameObjects[i].GetComponent<ConnectionLine>();
                line.connections[0] = connectionPoints[inst.connectionIDs[0]];
                line.connections[1] = connectionPoints[inst.connectionIDs[1]];

                HingeJoint2D temp0 = line.connections[0].gameObject.AddComponent<HingeJoint2D>();
                line.hingeJoints[0] = temp0;
                temp0.connectedBody = line.GetComponent<Rigidbody2D>();

                HingeJoint2D temp1 = line.connections[1].gameObject.AddComponent<HingeJoint2D>();
                line.hingeJoints[1] = temp1;
                temp1.connectedBody = line.GetComponent<Rigidbody2D>();
            }
            else
            {
                ConnectionPoint point = gameObjects[i].GetComponent<ConnectionPoint>();

                foreach (var conID in inst.connectionIDs)
                {
                    point.connections.Add(gameObjects[conID]);
                }
            }
        }
    }
}
