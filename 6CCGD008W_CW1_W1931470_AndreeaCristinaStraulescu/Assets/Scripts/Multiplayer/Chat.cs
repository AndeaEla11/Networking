using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using Photon.Pun;

public class Chat : MonoBehaviour, IChatClientListener
{
    public string userName;
    public ChatClient ChatClient;

    public InputField InputField;
    public Text ChatContent; 

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log("Chat - " + level + " - " + message);
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("Chat - OnChatStateChange" + state);
    }

    public void OnConnected()
    {
        Debug.Log("Chat - User:" + userName + " has connected");
        ChatClient.Subscribe(PhotonNetwork.CurrentRoom.Name, creationOptions: new ChannelCreationOptions() { PublishSubscribers = true });
        
    }

    public void OnDisconnected()
    {
        Debug.Log("Chat - User:" + userName + " has disconnected");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        ChatChannel currentChat; 
        if (ChatClient.TryGetChannel(PhotonNetwork.CurrentRoom.Name, out currentChat))
        {
            ChatContent.text = currentChat.ToStringMessages(); 
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            if (results[i])
            {
                Debug.Log("Chat - Subscribed to " + channels[i] + "channel");
                ChatClient.PublishMessage(PhotonNetwork.CurrentRoom.Name, " has joined the chat!");
            }
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
    }

    public void OnUserSubscribed(string channel, string user)
    {
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChatClient = new ChatClient(this);
    }

    // Update is called once per frame
    void Update()
    {
        ChatClient.Service(); 
    }

    public void SetMessage()
    {
        if (InputField.text == "")
            return;
        ChatClient.PublishMessage(PhotonNetwork.CurrentRoom.Name, InputField.text);
        InputField.text = ""; 
    }
}
