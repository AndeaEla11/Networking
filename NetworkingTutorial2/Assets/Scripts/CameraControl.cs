using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform playerCharacter;
    Vector3 cameraOffset;

    void Start()
    {
        cameraOffset = transform.position - playerCharacter.position;
    }

    void Update()
    {
        transform.position = playerCharacter.position + cameraOffset;
    }
}
