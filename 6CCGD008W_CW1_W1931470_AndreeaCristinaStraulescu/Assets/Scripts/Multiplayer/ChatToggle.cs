using UnityEngine;

public class ChatToggle : MonoBehaviour
{
    public GameObject chatPanel;
    public KeyCode toggleKey = KeyCode.T;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleChat();
        }
    }

    void ToggleChat()
    {
        if (chatPanel == null) return;

        chatPanel.SetActive(!chatPanel.activeSelf);
    }
}