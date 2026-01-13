using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections.Generic;
using System;

public class GlobalLeaderboard : MonoBehaviour
{
    public LeaderboardPopup leaderboardPopup;

    public void SubmitScore(int playerScore)
    {
        UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate()
                {
                    StatisticName = "Most Kills",
                    Value = playerScore,
                }
            }
        };
    
        PlayFabClientAPI.UpdatePlayerStatistics(request, PlayFabUpdateStatsResult, PlayFabUpdateStatsError); 

        void PlayFabUpdateStatsResult(UpdatePlayerStatisticsResult updatePlayerStatisticsResult)
        {
            Debug.Log("PlayFab - Score submitted.");
        }

        void PlayFabUpdateStatsError(PlayFabError updatePlayerStatisticsError)
        {
            Debug.Log("PlayFab - Error occurred while submitting score: " + updatePlayerStatisticsError.ErrorMessage);
        }

    }

    internal void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Most Kills",
            StartPosition = 0,
            MaxResultsCount = 5
        };

        PlayFabClientAPI.GetLeaderboard(request, PlayFabGetLeaderboardResult, PlayFabGetLeaderboardError);

        void PlayFabGetLeaderboardResult(GetLeaderboardResult getLeaderboardResult)
        {
            Debug.Log("PlayFab - Get Leaderboard completed.");
            leaderboardPopup.UpdateUI(getLeaderboardResult.Leaderboard);
        }

        void PlayFabGetLeaderboardError(PlayFabError error)
        {
            Debug.Log("PlayFab - Error getting leaderboard: " + error.ErrorMessage);
        }
    }

}
