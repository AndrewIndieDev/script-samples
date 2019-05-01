using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Valve.VR;

public class MoCap : MonoBehaviour
{
    [System.Serializable]
    public class MoCapFrame
    {
        public MoCapPos hmdPos;
        public MoCapPos leftPos;
        public MoCapPos rightPos;

        public MoCapRot hmdRot;
        public MoCapRot leftRot;
        public MoCapRot rightRot;

        public MoCapFrame(MoCapPos hmdPos, MoCapPos leftPos, MoCapPos rightPos, MoCapRot hmdRot, MoCapRot leftRot, MoCapRot rightRot)
        {
            this.hmdPos = hmdPos;
            this.leftPos = leftPos;
            this.rightPos = rightPos;
            this.hmdRot = hmdRot;
            this.leftRot = leftRot;
            this.rightRot = rightRot;
        }
    }

    #region Variables
    public SteamVR_Action_Boolean moCap_Start;
    public SteamVR_Action_Boolean moCap_End;
    public Transform hmd;
    public Transform leftController;
    public Transform rightController;

    private Coroutine recordingCoroutine;

    public float fps = 30;

    private List<MoCapFrame> record = new List<MoCapFrame>();
    private bool recording;
    #endregion

    // Use this for initialization
    private void Awake()
    {
        moCap_Start[SteamVR_Input_Sources.RightHand].onStateUp += StartRecording;
        moCap_Start[SteamVR_Input_Sources.LeftHand].onStateUp += EndRecording;
    }

    private void OnDestroy()
    {
        moCap_Start[SteamVR_Input_Sources.RightHand].onStateUp -= StartRecording;
        moCap_Start[SteamVR_Input_Sources.LeftHand].onStateUp -= EndRecording;
    }

    private void StartRecording(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
        recording = true;

        Debug.Log("Started recording");
        recordingCoroutine = StartCoroutine(Recording());
    }

    private IEnumerator Recording()
    {
        while (recording)
        {
            yield return new WaitForSeconds(1f / fps);

            MoCapFrame t = new MoCapFrame(
                hmd.localPosition.ToMoCapPos(),
                leftController.localPosition.ToMoCapPos(),
                rightController.localPosition.ToMoCapPos(),
                hmd.localRotation.ToMoCapRot(),
                leftController.localRotation.ToMoCapRot(),
                rightController.localRotation.ToMoCapRot()
                );

            record.Add(t);
        }
    }

    private void EndRecording(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
        recording = false;

        Debug.Log("Stopped recording");
        StopCoroutine(recordingCoroutine);

#if UNITY_EDITOR
        string path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save as", "RecordingName", "json", "Save recording");
        if (!string.IsNullOrEmpty(path))
        {
            SaveRecording(path);
        }
#endif
    }

    private void SaveRecording(string path)
    {
        JsonFile.SerializeMoCapList(record, path);
    }
}

[System.Serializable]
public class MoCapPos
{
    public float x, y, z;

    public MoCapPos(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

[System.Serializable]
public class MoCapRot
{
    public float x, y, z, w;

    public MoCapRot(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }
}

public static class Extensions
{
    public static MoCapPos ToMoCapPos(this Vector3 v)
    {
        return new MoCapPos(v.x, v.y, v.z);
    }

    public static MoCapRot ToMoCapRot(this Quaternion v)
    {
        return new MoCapRot(v.x, v.y, v.z, v.w);
    }

    public static Vector3 ToVector3(this MoCapPos v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

    public static Quaternion ToQuaternion(this MoCapRot v)
    {
        return new Quaternion(v.x, v.y, v.z, v.w);
    }
}