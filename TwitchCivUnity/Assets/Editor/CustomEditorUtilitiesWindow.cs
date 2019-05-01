using UnityEngine;
using System.Collections;
using UnityEditor;

public class CustomEditorUtilitiesWindow : EditorWindow {

    public bool startOnClick = false;
    GameObject player;
    Vector3 startPos;
    Vector3 tempPos;
    LayerMask ignoreLayer;

    static float xRmin = -180;
    static float yRmin = -180;
    static float zRmin = -180;

    static float xRmax = 180;
    static float yRmax = 180;
    static float zRmax = 180;

    static float xSmin = 0.9f;
    static float ySmin = 0.9f;
    static float zSmin = 0.9f;

    static float xSmax = 1.1f;
    static float ySmax = 1.1f;
    static float zSmax = 1.1f;

    static bool equalScale = true;

    [MenuItem("Window/Editor Utilities")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CustomEditorUtilitiesWindow));
    }

	// Use this for initialization
	void OnEnable () {
        SceneView.onSceneGUIDelegate += SceneGUI;
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            startPos = player.transform.position;
        }
    }
	
	// Update is called once per frame
	void SceneGUI (SceneView sceneView) {
        Event cur = Event.current;

        if (cur.type == EventType.MouseDown && cur.button == 0 && startOnClick && player != null && startPos != null)
        {
            startOnClick = false;
            player.GetComponent<CapsuleCollider>().enabled = true;
            UnityEditor.EditorApplication.isPlaying = true;
        }
        if (startOnClick)
        {
            StartGame(cur.mousePosition);
        }
	}

    void OnGUI()
    {
        GUILayout.Label("Teleport Player");
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(startOnClick);
        if (GUILayout.Button("Start From"))
        {
            startOnClick = true;
            if (player != null && startPos != null)
            {
                player.GetComponent<CapsuleCollider>().enabled = false;
                tempPos = player.transform.position;
            }
        }
        EditorGUI.EndDisabledGroup();
        EditorGUI.BeginDisabledGroup(!startOnClick);
        if (GUILayout.Button("Cancel"))
        {
            startOnClick = false;
            if (player != null && startPos != null)
            {
                player.GetComponent<CapsuleCollider>().enabled = true;
                player.transform.position = tempPos;
            }
        }
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();
        EditorGUI.BeginDisabledGroup(startOnClick);
        if (GUILayout.Button("Reset Player Position"))
        {
            if (player != null && startPos != null)
            {
                player.transform.position = startPos;
            }
        }
        EditorGUI.EndDisabledGroup();
        GUILayout.EndVertical();

        GUILayout.Space(20);

        GUILayout.Label("Randomize Object");
        GUILayout.BeginVertical("box");
        GUILayout.Label("Rotation");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Min X");
        xRmin = EditorGUILayout.FloatField(xRmin);
        GUILayout.Label("Min Y");
        yRmin = EditorGUILayout.FloatField(yRmin);
        GUILayout.Label("Min Z");
        zRmin = EditorGUILayout.FloatField(zRmin);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Max X");
        xRmax = EditorGUILayout.FloatField(xRmax);
        GUILayout.Label("Max Y");
        yRmax = EditorGUILayout.FloatField(yRmax);
        GUILayout.Label("Max Z");
        zRmax = EditorGUILayout.FloatField(zRmax);
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Randomize Rotation"))
        {
            RandomizeRotation();
        }
        GUILayout.Space(10);
        GUILayout.Label("Scale");
        equalScale = GUILayout.Toggle(equalScale, "Equal axis scale");
        if (!equalScale)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Min X");
            xSmin = EditorGUILayout.FloatField(xSmin);
            GUILayout.Label("Min Y");
            ySmin = EditorGUILayout.FloatField(ySmin);
            GUILayout.Label("Min Z");
            zSmin = EditorGUILayout.FloatField(zSmin);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Max X");
            xSmax = EditorGUILayout.FloatField(xSmax);
            GUILayout.Label("Max Y");
            ySmax = EditorGUILayout.FloatField(ySmax);
            GUILayout.Label("Max Z");
            zSmax = EditorGUILayout.FloatField(zSmax);
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Min");
            xSmin = EditorGUILayout.FloatField(xSmin);
            GUILayout.Label("Max");
            xSmax = EditorGUILayout.FloatField(xSmax);
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Randomize Scale"))
        {
            RandomizeScale();
        }
        GUILayout.EndVertical();
    }

    void StartGame(Vector2 mPos)
    {
        if (player != null)
        {
            Ray ray = Camera.current.ScreenPointToRay(new Vector3(mPos.x, -mPos.y + Screen.height - 38,0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 9999, ~ignoreLayer))
            {
                player.transform.position = hit.point + Vector3.up * player.GetComponent<CapsuleCollider>().height/2;
            }
        }
        else
        {
            startOnClick = false;
            EditorUtility.DisplayDialog("Error","No player found - make sure player is tagged as \"Player\"","Ok, understood, Master David");
        }
    }

    void OnDisable()
    {
        if (player != null && startPos != null)
        {
            player.transform.position = startPos;
        }
    }

    void RandomizeRotation()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            Undo.RecordObject(Selection.gameObjects[i].transform, "Random Rotation");
            Selection.gameObjects[i].transform.rotation = Quaternion.Euler(Random.Range(xRmin, xRmax), Random.Range(yRmin, yRmax), Random.Range(zRmin, zRmax));
        }
    }

    void RandomizeScale()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            if (!equalScale)
            {
                Undo.RecordObject(Selection.gameObjects[i].transform, "Random Scale");
                Selection.gameObjects[i].transform.localScale = new Vector3(Random.Range(xSmin, xSmax), Random.Range(ySmin, ySmax), Random.Range(zSmin, zSmax));
            }
            else
            {
                Undo.RecordObject(Selection.gameObjects[i].transform, "Random Scale");
                float ran = Random.Range(xSmin, xSmax);
                Selection.gameObjects[i].transform.localScale = new Vector3(ran, ran, ran);
            }
        }
    }
}
