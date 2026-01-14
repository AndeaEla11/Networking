using UnityEngine;

public class ChatToggle : MonoBehaviour
{
    public GameObject chatPanel;
    public KeyCode toggleKey = KeyCode.T;

    public static bool IsChatOpen { get; private set; }

    void Start()
    {
        if (chatPanel != null)
            IsChatOpen = chatPanel.activeSelf;
    }

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

        bool newState = !chatPanel.activeSelf;
        chatPanel.SetActive(newState);
        IsChatOpen = newState;
    }
}