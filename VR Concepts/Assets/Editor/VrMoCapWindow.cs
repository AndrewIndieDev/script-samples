using UnityEngine;
using UnityEditor;

public class VrMoCapWindow : EditorWindow
{
    MoCap motionCapture;

    private void Awake()
    {
        MoCap moCap = FindObjectOfType<MoCap>();
        if (moCap == null) return;
        motionCapture = moCap;
    }

    [MenuItem("Window/Motion Capture")]
    public static void ShowWindow()
    {
        GetWindow<VrMoCapWindow>("Motion Capture");
    }

    private void OnGUI()
    {
        if (motionCapture == null) return;
        GUILayout.Label("Object name: " + motionCapture.transform.gameObject.name);
    }
}
