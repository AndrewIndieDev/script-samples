using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ItemType
{
    LOG,
    STONE,
    GOLD,
    SILVER,
    COPPER,
    COAL,
    FISH,
    BOOZE,
    WATER,
    MEAT
}

public class GameData : MonoBehaviour {

    public static GameData manager;
    public List<PlayerBehaviour> players = new List<PlayerBehaviour>();
    public GameObject playerBoxPrefab;
	public GameObject environmentParent;
    public GameObject[] spawnpoints;
    int spawnpointIndex = 0;

    void Start()
    {
        manager = this;
    }

    public void AddPlayer(string playerName)
    {
        foreach (PlayerBehaviour i in players)
        {
            if (i.name == playerName)
                return;
        }
        GameObject temp = (GameObject)Instantiate(playerBoxPrefab, (spawnpoints != null) ? spawnpoints[spawnpointIndex].transform.position : Vector3.zero, Quaternion.identity);
        players.Add(temp.GetComponentInChildren<PlayerBehaviour>());
        temp.GetComponentInChildren<PlayerBehaviour>().gameObject.name = playerName;
        temp.GetComponentInChildren<PlayerBehaviour>().name = playerName;
        spawnpointIndex += 1;
        if (spawnpointIndex >= spawnpoints.Length)
            spawnpointIndex = 0;
    }

    public void RemovePlayer(string playerName)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].name == playerName)
            {
                Destroy(players[i].gameObject);
                players.RemoveAt(i);
                return;
            }
        }
        IRCWindow.manager.SendChatMessage("/w " + playerName + " You are not in game anyway");
    }

    public void MakePlayerHappy(string playerName)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].name == playerName)
            {
                players[i].gameObject.GetComponent<Animator>().Play("Happy");
                return;
            }
        }
        IRCWindow.manager.SendChatMessage("/w "+playerName+ " You are not in game yet, please use !join to join");
    }
}
