using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine; 

public class LeaderboardPopup : MonoBehaviour
{
    public GameObject mostKillsHolder;
    public GameObject quickestWinHolder;

    public GameObject noScoreText;
    public GameObject leaderboardItem;

    private void OnEnable()
    {
        GameManager.instance.globalLeaderboard.GetBothLeaderboards();
    }

    public void UpdateMostKillsUI(List<PlayerLeaderboardEntry> playerLeaderboardEntries)
    {

            DestroyChildren(mostKillsHolder.transform);

            for (int i = 0; i < playerLeaderboardEntries.Count; i++)
            {
                GameObject newLeaderboardItem = Instantiate(leaderboardItem, Vector3.zero, Quaternion.identity, mostKillsHolder.transform);
                newLeaderboardItem.GetComponent<LeaderboardItem>().SetScores(i + 1, playerLeaderboardEntries[i].DisplayName, playerLeaderboardEntries[i].StatValue);
            }

            RefreshNoScoreText();
    }

    public void UpdateQuickestWinUI(List<PlayerLeaderboardEntry> playerLeaderboardEntries)
    {
        DestroyChildren(quickestWinHolder.transform);

        for (int i = 0; i < playerLeaderboardEntries.Count; i++)
        {
            GameObject newLeaderboardIt = Instantiate(leaderboardItem, Vector3.zero, Quaternion.identity, quickestWinHolder.transform);
            newLeaderboardIt.GetComponent<LeaderboardItem>().SetScores(i + 1, playerLeaderboardEntries[i].DisplayName, playerLeaderboardEntries[i].StatValue);
        }

        RefreshNoScoreText();
    }

    void DestroyChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    void RefreshNoScoreText()
    {
        bool hasMostKills = mostKillsHolder.transform.childCount > 0;
        bool hasQuickestWin = quickestWinHolder.transform.childCount > 0;

        if(!hasMostKills && !hasQuickestWin)
        {
            noScoreText.SetActive(true);
        }
        else
        {
            noScoreText.SetActive(false);
        }
    }
}
