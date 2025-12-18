using UnityEngine;
using Photon.Pun;
using UnityEngine.UI; 

public class PlayerNameUI : MonoBehaviour
{
    public Text nameText;
    public Transform target; 
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;

        if (Camera.main != null)
            transform.rotation = Camera.main.transform.rotation;
    }
}
