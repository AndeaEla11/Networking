using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using System; 

public class MultiplayerLevelManager : MonoBehaviourPunCallbacks 
{
    public int maxKills = 3;
    public GameObject gameOverPopup;
    public Text winnerText;
    public Text customMessageText;

    void Start()
    {
        PhotonNetwork.Instantiate("Multiplayer Player", new Vector3(0,1,0), Quaternion.identity); 
        
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer.GetScore() == maxKills)
        {
            winnerText.text = targetPlayer.NickName;
            gameOverPopup.SetActive(true);

            if (PhotonNetwork.LocalPlayer == targetPlayer)
            {
                customMessageText.text = "You won!";
            }
            else
            {
                customMessageText.text = "You lost!";
            }

            StorePersonalBest();
            
        }
    }

    void StorePersonalBest()
    {
        int currentScore = PhotonNetwork.LocalPlayer.GetScore();
        PlayerData playerData = GameManager.instance.playerData;

        if (currentScore > playerData.bestScore)
        {
            playerData.username = PhotonNetwork.LocalPlayer.NickName;
            playerData.bestScore = currentScore;
            playerData.bestScoreDate = DateTime.UtcNow.ToString();
            playerData.totalPlayersInGame = PhotonNetwork.CurrentRoom.PlayerCount;
            playerData.roomName = PhotonNetwork.CurrentRoom.Name;

            GameManager.instance.SavePlayerData(); 
        }
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom(); 
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("Lobby"); 
    }

}
