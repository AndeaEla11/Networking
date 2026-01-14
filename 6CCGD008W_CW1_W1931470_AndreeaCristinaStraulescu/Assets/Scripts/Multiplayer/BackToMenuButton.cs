using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class BackToMenuButton : MonoBehaviour
{
    public string menuSceneName = "MainMenu";

    public void BackToMenu()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneManager.LoadScene(menuSceneName);
        }
    }

    public void OnLeftRoom()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
