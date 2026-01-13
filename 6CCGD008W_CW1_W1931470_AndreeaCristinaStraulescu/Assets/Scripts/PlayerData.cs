using System;
using UnityEngine;

public class PlayerData
{
    public string uid; 
    public string username;
    public int bestScore;
    public string bestScoreDate;
    public int totalPlayersInGame;
    public string roomName; 

    public PlayerData()
    {
        uid = Guid.NewGuid().ToString();
    }
}
