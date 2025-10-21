using UnityEngine;

public class BulletController : MonoBehaviour
{
    Rigidbody rb;

    public float bulletSpeed = 15f;

    public AudioClip BulletHitAudio; 

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void InitializeBullet(Vector3 originalDirection)
    {
        transform.forward = originalDirection;
        rb.linearVelocity = transform.forward * bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.Play3D(BulletHitAudio, transform.position);

        Destroy(gameObject);
    }
}
