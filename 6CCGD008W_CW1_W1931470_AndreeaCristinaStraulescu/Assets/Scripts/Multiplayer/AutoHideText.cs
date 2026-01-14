using UnityEngine;

public class AutoHideText : MonoBehaviour
{
    public float seconds = 5f;

    void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(Hide), seconds);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
