using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BuildingBlocks))]
public class BuildingBlocksInspector : Editor
{
    private BuildingBlocks t;
    private SerializedProperty blockList;

    private bool blocksOpen = false;
    private bool materialsOpen = false;

    // Use this for initialization
    void OnEnable ()
    {
        blocksOpen = PlayerPrefs.GetInt("blocksOpen", 0) == 1;
        materialsOpen = PlayerPrefs.GetInt("materialsOpen", 0) == 1;
        t = (BuildingBlocks) target;
	}

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        blocksOpen = EditorGUILayout.Foldout(blocksOpen, "Blocks", true);
        if (EditorGUI.EndChangeCheck())
        {
            PlayerPrefs.SetInt("blocksOpen", blocksOpen ? 1 : 0);
        }
        if (blocksOpen)
        {
            EditorGUI.BeginChangeCheck();
            for (var i = 0; i < t.blocks.Count; ++i)
            {
                GUILayout.BeginHorizontal("Box");
                t.blocks[i].name = EditorGUILayout.TextField(t.blocks[i].name);
                t.blocks[i].description = EditorGUILayout.TextField(t.blocks[i].description);
                t.blocks[i].gameObject = (GameObject)EditorGUILayout.ObjectField(t.blocks[i].gameObject, typeof(GameObject), true);
                t.blocks[i].category = (BuildingBlocks.EBuildingBlockCategory)EditorGUILayout.EnumPopup(t.blocks[i].category);
                if (GUILayout.Button("X", GUILayout.Width(20))) { t.blocks.RemoveAt(i); return; }
                GUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed blocks");
            }
            if (GUILayout.Button("Add Block"))
            {
                t.blocks.Add(new BuildingBlocks.Block());
            }
        }
        
        EditorGUI.BeginChangeCheck();
        materialsOpen = EditorGUILayout.Foldout(materialsOpen, "Materials", true);
        if (EditorGUI.EndChangeCheck())
        {
            PlayerPrefs.SetInt("materialsOpen", materialsOpen ? 1 : 0);
        }
        if (materialsOpen)
        {
            for (var i = 0; i < t.presetMaterials.Count; ++i)
            {
                GUILayout.BeginHorizontal("Box");
                t.presetMaterials[i] = (Material)EditorGUILayout.ObjectField(t.presetMaterials[i], typeof(Material), true);
                if (GUILayout.Button("X", GUILayout.Width(20))) { t.presetMaterials.RemoveAt(i); return; }
                GUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add Material"))
            {
                t.presetMaterials.Add(null);
            }
        }
    }
}
