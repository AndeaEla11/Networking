using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections.Generic;
using System;

public class GlobalLeaderboard : MonoBehaviour
{
    public LeaderboardPopup leaderboardPopup;

    public void GetBothLeaderboards()
    {
        GetMostKillsLeaderboard();
        GetQuickestWinLeaderboard();
    }

    public void SubmitMostKills(int killsNumber)
    {
        UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Most Kills",
                    Value = killsNumber,
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

    public void SubmitQuickestWin(int timeSeconds)
    {
        UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>
        {
            new StatisticUpdate
            {
                StatisticName = "Quickest Win",
                Value = timeSeconds,
            }
        }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, PlayFabUpdateStatsResult, PlayFabUpdateStatsError);

        void PlayFabUpdateStatsResult(UpdatePlayerStatisticsResult updatePlayerStatisticsResult)
        {
            Debug.Log("PlayFab - Quickest Win submitted: " + timeSeconds);
        }

        void PlayFabUpdateStatsError(PlayFabError updatePlayerStatisticsError)
        {
            Debug.Log("PlayFab - Error submitting Quickest Win: " + updatePlayerStatisticsError.ErrorMessage);
        }
    }

    internal void GetMostKillsLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Most Kills",
            StartPosition = 0,
            MaxResultsCount = 5
        };

        PlayFabClientAPI.GetLeaderboard(request, PlayFabGetMostKillsResult, PlayFabGetMostKillsError);

        void PlayFabGetMostKillsResult(GetLeaderboardResult getLeaderboardResult)
        {
            Debug.Log("PlayFab - Get Leaderboard completed.");
            leaderboardPopup.UpdateMostKillsUI(getLeaderboardResult.Leaderboard);
        }

        void PlayFabGetMostKillsError(PlayFabError error)
        {
            Debug.Log("PlayFab - Error getting leaderboard: " + error.ErrorMessage);
        }
    }

    void GetQuickestWinLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Quickest Win",
            StartPosition = 0,
            MaxResultsCount = 5
        };

        PlayFabClientAPI.GetLeaderboard(request, PlayFabGetQuickestWinResult, PlayFabGetQuickestWinError);

        void PlayFabGetQuickestWinResult(GetLeaderboardResult result)
        {
            Debug.Log("PlayFab - Quickest Win leaderboard completed.");
            leaderboardPopup.UpdateQuickestWinUI(result.Leaderboard);
        }

        void PlayFabGetQuickestWinError(PlayFabError error)
        {
            Debug.Log("PlayFab - Error getting Quickest Win: " + error.ErrorMessage);
        }
    }

}
