using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerViewer : EditorWindow
{
    private PlayerBehaviour[] players;
    private Transform prevCamPos;
    private bool running = false;

    [MenuItem("Window/PlayerViewer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(PlayerViewer));
    }

	// Use this for initialization
	void OnEnable ()
	{
	    RefreshPlayers();
	}

    void Update()
    {
        if (Application.isPlaying && !running)
        {
            Debug.Log("Started");
            RefreshPlayers();
        }
        running = Application.isPlaying;
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh players"))
            RefreshPlayers();
        if (GUILayout.Button("Stop Spectating"))
            StopSpectating();
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        GUILayout.Label("Players:");
        for (int i = 0; i < players.Length; i++)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(players[i].name))
                GotoPlayer(i);
            EditorGUILayout.LabelField(players[i].currentJobID.ToString(), GUILayout.Width(100));
            GUILayout.EndHorizontal();
        }
    }

    void RefreshPlayers()
    {
        players = GameObject.FindObjectsOfType<PlayerBehaviour>();
    }

    void GotoPlayer(int index)
    {
        if (players[index] != null)
        {
            if (Camera.main.gameObject.GetComponent<FlyCamera>().spectate == false)
            {
                prevCamPos = Camera.main.transform;
            }
            Camera.main.gameObject.GetComponent<FlyCamera>().spectatePlayer = players[index].transform;
            Camera.main.gameObject.GetComponent<FlyCamera>().spectate = true;
        }
        else
        {
            RefreshPlayers();
        }
    }

    void StopSpectating()
    {
        Camera.main.gameObject.GetComponent<FlyCamera>().spectate = false;
        if (prevCamPos != null)
        {
            Camera.main.transform.position = prevCamPos.position;
            Camera.main.transform.rotation = prevCamPos.rotation;
        }
    }
}
