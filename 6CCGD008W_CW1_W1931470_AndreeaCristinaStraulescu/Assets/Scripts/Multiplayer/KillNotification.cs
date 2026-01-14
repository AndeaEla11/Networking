using UnityEngine;
using System.Collections;
using TMPro;

public class KillNotification : MonoBehaviour
{
    public TextMeshProUGUI killText;
    public float displayTime = 2f;

    void Start()
    {
        killText.text = "";
    }

    public void Show(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(message));
    }

    IEnumerator ShowRoutine(string message)
    {
        killText.text = message;
        yield return new WaitForSeconds(displayTime);
        killText.text = "";
    }
}
