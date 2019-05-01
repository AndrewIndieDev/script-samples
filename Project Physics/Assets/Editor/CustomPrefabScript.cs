using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class DescriptionPopupWindow : EditorWindow
{
    public Master target;
    public int editIndex;
    public string description;

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("Change description for "+target.editorItems[editIndex].name);
        GUILayout.Space(5);
        description = GUILayout.TextArea(description);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply"))
        {
            Undo.RecordObject(target, "Changed Description");
            target.editorItems[editIndex].description = description;
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
}

[CustomEditor(typeof(Master))]
public class CustomPrefabScript : Editor {

    Master t;

    void Awake()
    {
        t = (Master)target;

        while (t.userItems.Count < Enum.GetValues(typeof(EBuildType)).Length)
        {
            Undo.RecordObject(t, "Added Useritem");
            t.userItems.Add(new Master.UserItem());
        }

        while (t.userItems.Count > Enum.GetValues(typeof(EBuildType)).Length)
        {
            Undo.RecordObject(t, "Removed Useritem");
            t.userItems.RemoveAt(t.userItems.Count-1);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(30);
        EditorGUILayout.LabelField("User Items", EditorStyles.boldLabel);
        GUILayout.BeginVertical("Box");
        for (int i = 0; i < t.userItems.Count; ++i)
        {
            if (i == t.userItems.Count -1) { continue; }

            bool tempConnectable = t.userItems[i].connectable;
            GameObject tempBuildTypePrefab = t.userItems[i].buildTypePrefab;
            Texture2D tempGuiVisualPrefab = t.userItems[i].guiVisualPrefab;

            EditorGUI.BeginChangeCheck();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(Enum.GetNames(typeof(EBuildType))[i]);
            EditorGUILayout.LabelField((i).ToString(), GUILayout.Width(40));
            tempConnectable = EditorGUILayout.Toggle(tempConnectable);
            tempBuildTypePrefab = (GameObject)EditorGUILayout.ObjectField(tempBuildTypePrefab, typeof(GameObject), false);
            tempGuiVisualPrefab = (Texture2D)EditorGUILayout.ObjectField(tempGuiVisualPrefab, typeof(Texture2D), false);
            GUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "ValueChanged");
                t.userItems[i].connectable = tempConnectable;
                t.userItems[i].buildTypePrefab = tempBuildTypePrefab;
                t.userItems[i].guiVisualPrefab = tempGuiVisualPrefab;
            }
        }
        if (GUILayout.Button("Add UserItem"))
        {
            Undo.RecordObject(target, "Added UserItem");
            t.userItems.Add(new Master.UserItem());
        }
        if (GUILayout.Button("Generate Icons"))
        {
            GenerateIcons();
        }
        GUILayout.EndVertical();
        GUILayout.Space(30);
        EditorGUILayout.LabelField("Editor Items", EditorStyles.boldLabel);
        GUILayout.BeginVertical("Box");
        for (int i = 0; i < t.editorItems.Count; ++i)
        {
            GUILayout.BeginHorizontal();
            t.editorItems[i].name = GUILayout.TextField(t.editorItems[i].name, GUILayout.Width(Screen.width/3f));
            t.editorItems[i].chainedScaling = GUILayout.Toggle(t.editorItems[i].chainedScaling, "");
            if (GUILayout.Button("Desc."))
            {
                DescriptionPopupWindow window = (DescriptionPopupWindow)EditorWindow.GetWindow(typeof(DescriptionPopupWindow));
                window.target = t;
                window.editIndex = i;
                window.description = t.editorItems[i].description;
                window.Show();
            }
            t.editorItems[i].gameObject = (GameObject)EditorGUILayout.ObjectField(t.editorItems[i].gameObject, typeof(GameObject), true);
            t.editorItems[i].guiTexture = (Texture2D)EditorGUILayout.ObjectField(t.editorItems[i].guiTexture, typeof(Texture2D), true);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                t.editorItems.RemoveAt(i);
                return;
            }
            GUILayout.EndHorizontal();
        }
        
        if (GUILayout.Button("Add EditorItem"))
        {
            Undo.RecordObject(target, "Added EditorItem");
            t.editorItems.Add(new Master.EditorItem());
        }
        if (GUILayout.Button("Generate Icons"))
        {
            GenerateIcons(true);
        }
        GUILayout.EndVertical();

        Undo.FlushUndoRecordObjects();
    }

    void GenerateIcons(bool forEditor = false)
    {
        if (!forEditor)
        {
            for (int i = 0; i < t.userItems.Count; i++)
            {
                try
                {
                    if (t.userItems[i] != null)
                    {
                        byte[] bytes = AssetPreview.GetAssetPreview(t.userItems[i].buildTypePrefab.gameObject).EncodeToPNG();
                        File.WriteAllBytes(Application.dataPath + "/Prefabs/Buildable Objects/GUI Visuals/" + i.ToString() + ".png", bytes);
                        string path = "Assets/Prefabs/Buildable Objects/GUI Visuals/" + i.ToString() + ".png";
                        t.userItems[i].guiVisualPrefab = (Texture2D)EditorGUIUtility.Load(path);
                    }
                    else
                    {
                        if (i == 0 || i == 1)
                        {
                            string path = "Assets/Prefabs/Buildable Objects/GUI Visuals/" + i.ToString() + ".png";
                            var tUserItem = t.userItems[i];
                            if (tUserItem != null)
                                tUserItem.guiVisualPrefab = (Texture2D) EditorGUIUtility.Load(path);
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }
        else
        {
            for (int i = 0; i < t.editorItems.Count; i++)
            {
                try
                {
                    if (t.editorItems[i] != null)
                    {
                        byte[] bytes = AssetPreview.GetAssetPreview(t.editorItems[i].gameObject).EncodeToPNG();
                        File.WriteAllBytes(Application.dataPath + "/Prefabs/World Blocks/GUI Visuals/" + i.ToString() + ".png", bytes);
                        string path = "Assets/Prefabs/World Blocks/GUI Visuals/" + i.ToString() + ".png";
                        t.editorItems[i].guiTexture = (Texture2D)EditorGUIUtility.Load(path);
                    }
                    else
                    {
                        if (i == 0 || i == 1)
                        {
                            string path = "Assets/Prefabs/World Blocks/GUI Visuals/" + i.ToString() + ".png";
                            var tEditorItem = t.editorItems[i];
                            if (tEditorItem != null)
                                tEditorItem.guiTexture = (Texture2D) EditorGUIUtility.Load(path);
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }
        AssetDatabase.Refresh();
    }
}