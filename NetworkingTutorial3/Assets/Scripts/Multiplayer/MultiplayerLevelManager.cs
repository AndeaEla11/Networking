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
    public Text timerText;

    private const string PlayAgainKey = "PlayAgainVote";

    public float matchDuration = 120f;   
    float timer;
    bool matchEnded = false;


    void Start()
    {
        if (gameOverPopup != null)
            gameOverPopup.SetActive(false);

        PhotonNetwork.Instantiate("Multiplayer Player",
            new Vector3(0, 1, 0),
            Quaternion.identity);

        timer = matchDuration;
        matchEnded = false;
    }

    void Update()
    {
        if (matchEnded)
            return;

        UpdateTimerUI();

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 0f;
            EndMatchByTime();
        }
    }

    void UpdateTimerUI()
    {
        if (timerText == null)
            return;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void EndMatchByTime()
    {
        matchEnded = true;

        Photon.Realtime.Player winner = null;
        int highestScore = -1;

        foreach (var p in PhotonNetwork.PlayerList)
        {
            int playerScore = p.GetScore();

            if (playerScore > highestScore)
            {
                highestScore = playerScore;
                winner = p;
            }
        }

        ShowGameOver(winner);
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey(PlayAgainKey))
        {
            CheckReplayVotes();
        }

        if (matchEnded)
            return;

        if (targetPlayer.GetScore() >= maxKills)
        {
            matchEnded = true;
            ShowGameOver(targetPlayer);
        }
    }

    void ShowGameOver(Photon.Realtime.Player winner)
    {
        winnerText.text = winner.NickName;
        gameOverPopup.SetActive(true);

        customMessageText.text = (PhotonNetwork.LocalPlayer == winner)
            ? "You won!"
            : "You lost!";

        StorePersonalBest();
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

    
    public void OnClickPlayAgain()
    {
        SendPlayAgainVote(true);
    }

    
    public void OnClickLeaveGame()
    {
        LeaveGame();
    }

    void SendPlayAgainVote(bool playAgain)
    {
        var h = new Hashtable
        {
            [PlayAgainKey] = playAgain
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(h);
    }

    void CheckReplayVotes()
    {
        int yesVotes = 0;
        int votedCount = 0;

        foreach (var p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties.TryGetValue(PlayAgainKey, out object value) && value is bool vote)
            {
                votedCount++;
                if (vote) yesVotes++;
            }
        }

        
        if (PhotonNetwork.IsMasterClient &&
            votedCount == PhotonNetwork.PlayerList.Length &&
            yesVotes >= 2)
        {
            photonView.RPC(nameof(RpcRestartRound), RpcTarget.All);
        }
    }

    [PunRPC]
    void RpcRestartRound()
    {
        timer = matchDuration;
        matchEnded = false;
        UpdateTimerUI();

        if (gameOverPopup != null)
            gameOverPopup.SetActive(false);

        
        ResetPlayersInScene();

        
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var p in PhotonNetwork.PlayerList)
            {
                p.SetScore(0);

                var reset = new Hashtable
                {
                    [PlayAgainKey] = null
                };
                p.SetCustomProperties(reset);
            }
        }
    }

    void ResetPlayersInScene()
    {
        var players = FindObjectsByType<Multiplayer>(FindObjectsSortMode.None);

        foreach (var player in players)
        {
            player.health = 100;
            if (player.healthBar != null)
                player.healthBar.value = player.health;

            player.transform.position = new Vector3(0, 1, 0);
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
