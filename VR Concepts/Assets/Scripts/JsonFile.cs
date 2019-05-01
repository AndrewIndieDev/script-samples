using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonFile
{
    public static void SerializeMoCapList(List<MoCap.MoCapFrame> list, string path)
    {
        StreamWriter sw = new StreamWriter(path);
        foreach (var frame in list)
        {
                sw.WriteLine(JsonUtility.ToJson(frame));
        }
        Debug.Log("Finished saving");
    }

    public static List<MoCap.MoCapFrame> DeserializeMoCapList(TextAsset txt)
    {
        List<MoCap.MoCapFrame> anim = new List<MoCap.MoCapFrame>();

        using (StreamReader sr = new StreamReader(Application.dataPath + "/Recordings/" + txt.name + ".json"))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (!line.EndsWith("}")) break;

                anim.Add(JsonUtility.FromJson<MoCap.MoCapFrame>(line));
            }
        }

        return anim;
    }
}
