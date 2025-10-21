using UnityEngine;
using UnityEngine.UI; 

public class Enemy : MonoBehaviour
{

    public float fireRate = 0.75f;
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    float nextFire;
    public GameObject bulletFiringEffect;
    public AudioClip playerShootingAudio;

    [HideInInspector]
    public int health = 100;
    public Slider healthBar; 

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
            {
            transform.LookAt(other.transform);
            Fire(); 
            }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletController bullet = collision.gameObject.GetComponent<BulletController>();
            TakeDamage(bullet.damage); 
        }
    }
    void TakeDamage (int damage)
    {
        health -= damage;
        healthBar.value = health;
        if (health <= 0)
            EnemyDied(); 
        
    }

    void EnemyDied ()
    {
        gameObject.SetActive(false);
    }


    void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);

            bullet.GetComponent<BulletController>()?.InitializeBullet(transform.rotation * Vector3.forward);

            AudioManager.Instance.Play3D(playerShootingAudio, transform.position);

            VFXManager.Instance.PlayVFX(bulletFiringEffect, bulletPosition.position);

        }
    }

}
