using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public enum ServerCommandAction
{
    JOIN,
    QUIT,
    HAPPY
}

public class IRCWindow : MonoBehaviour
{

    private void DecodeMessage(string message, string sender)
    {
        if (message.Contains("!help"))
        {
            SendChatMessage("Commands: \"!join\", \"!quit\", \"!happy\"");
        }

        if (message.Contains("!join"))
        {
            serverCommands.Add(new ServerCommand(sender, ServerCommandAction.JOIN));
        }

        if (message.Contains("!quit"))
        {
            serverCommands.Add(new ServerCommand(sender, ServerCommandAction.QUIT));
        }

        if (message.Contains("!happy"))
        {
            serverCommands.Add(new ServerCommand(sender, ServerCommandAction.HAPPY));
        }
    }

    private void ProcessCommands(ref List<ServerCommand> commands)
    {
        while (serverCommands.Count > 0)
        {
            ServerCommand currentIndex = serverCommands[0];
            switch (currentIndex.action)
            {
                case ServerCommandAction.JOIN:
                    GameData.manager.AddPlayer(currentIndex.playerName);
                    break;

                case ServerCommandAction.QUIT:
                    GameData.manager.RemovePlayer(currentIndex.playerName);
                    break;

                case ServerCommandAction.HAPPY:
                    GameData.manager.MakePlayerHappy(currentIndex.playerName);
                    break;

                default:
                    Debug.Log("Unknown action \"" + currentIndex.action + "\" for player \"" + currentIndex.playerName + "\"!");
                    break;
            }
            serverCommands.RemoveAt(0);
        }
    }

    #region Variables
    private Socket socket;
    public string IP = "irc.chat.twitch.tv";
    public int Port = 6667;
    public string Channel = "#xxncdavexx";
    public string Username = "xxncdavexx";
    public Text chat;
    List<string> queueMessages = new List<string>();
    List<string> inChat = new List<string>();
    List<ServerCommand> serverCommands = new List<ServerCommand>();
    AudioSource AS;
    public static IRCWindow manager;
    #endregion

    void Start()
    {
        queueMessages.Add("Connecting to chat...");
        Connect();
        AS = GetComponent<AudioSource>();
        manager = this;
    }

    private void ProcessMessage(string msg)
    {
        #region Message divider
        Debug.Log(msg);
        if (msg.Contains("PRIVMSG"))
        {
            string sender = "";
            string senderColor = "";
            string message = "";

            int nameIndex = msg.IndexOf("display-name=") + "display-name=".Length;
            if (nameIndex > -1)
            {
                int nameEndIndex = msg.IndexOf(';', nameIndex);
                sender = msg.Substring(nameIndex, nameEndIndex - nameIndex);
            }

            int colorIndex = msg.IndexOf("color=") + "color=".Length;
            if (colorIndex > -1)
            {
                int nameEndIndex = msg.IndexOf(';', colorIndex);
                senderColor = msg.Substring(colorIndex, nameEndIndex - colorIndex);
                if (senderColor == "")
                    senderColor = "#00FF00";
            }

            int privmsgIndex = msg.IndexOf("PRIVMSG");
            int msgstartIndex = msg.IndexOf(':', privmsgIndex) + 1;
            message = msg.Substring(msgstartIndex);
            queueMessages.Add("<color="+senderColor+"FF>" + sender + "</color>" + ": " + message);
#endregion
            //message is the message
            //sender is the person who sent the message
            //sendercolor is the color of the sender's name

            DecodeMessage(message, sender);
        }

        #region Ping
        if (msg.Contains("PING"))
        {
            Debug.Log("RECEIVED PING, NEED TO SEND PONG");
            SendCommand("PONG tmi.twitch.tv");
        }
#endregion
    }

    void Update()
    {
        #region Queue Messages
        while (queueMessages.Count > 0)
        {
            AddToChat(queueMessages[0]);
            queueMessages.RemoveAt(0);
            AS.Play();
        }
#endregion

        ProcessCommands(ref serverCommands);
    }

    public void SendChatMessage(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            if (socket.Connected)
            {
                SendCommand("PRIVMSG " + Channel + " :" + message + "\r\n");
            }
        }
    }

    void Connect()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.BeginConnect(IP, Port, ConnectCallback, socket);
    }

    void Disconnect()
    {
        //socket.Disconnect(false);
        //socket.Shutdown(SocketShutdown.Both);
        socket.Close();
        Debug.Log("Disconnected!");
    }

    void AddToChat(string message)
    {
        inChat.Add(message);
        if (inChat.Count > 35)
        {
            inChat.RemoveAt(0);
        }
        chat.text = "";
        for(int i = 0;i < inChat.Count;i++)
        {
            chat.text += inChat[i];
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        StateObject state = (StateObject)ar.AsyncState;
        Socket client = state.workSocket;
        try
        {
            int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0)
            {
                string msg = Encoding.UTF8.GetString(state.buffer, 0, bytesRead);
                ProcessMessage(msg);
            }

            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, ReceiveCallback, state); ;
        }
        catch
        {
            Debug.LogError("Connection to server lost!");
            //Debug.LogError(e.Message);
        }
    }

    public void SendCommand(string msg)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(msg + "\r\n");
        socket.Send(buffer, buffer.Length, SocketFlags.None);
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        Socket client = (Socket)ar.AsyncState;


        try
        {
            client.EndConnect(ar);



            StateObject state = new StateObject();
            state.workSocket = client;
            //state.client = this;
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, ReceiveCallback, state);

            queueMessages.Add("\nConnected!\n");

            SendCommand("PASS oauth:d1sgkrkl9cmxu0empcvq82z1hubn6y"); //oauth
            SendCommand("USER " + Username);
            SendCommand("NICK " + Username);
            SendCommand("JOIN " + Channel);
            SendCommand("CAP REQ :twitch.tv/tags");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    void OnDisable()
    {
        Disconnect();
    }
}

public class StateObject
{
    // Client  socket.
    public Socket workSocket = null;
    // Size of receive buffer.
    public const int BufferSize = 1024;
    // Receive buffer.
    public byte[] buffer = new byte[BufferSize];
    // Received data string.
    public StringBuilder sb = new StringBuilder();
}

public class ServerCommand
{

    public string playerName;
    public ServerCommandAction action;

    public ServerCommand(string player, ServerCommandAction actionType)
    {
        playerName = player;
        action = actionType;
    }
}