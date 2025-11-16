using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class Multiplayer : MonoBehaviour, IPunObservable
{
    public float movementSpeed = 10f;
    public new Rigidbody rigidbody;

    [HideInInspector]
    public int health = 100;
    public Slider healthBar;

    PhotonView photonView;

    public float fireRate = 0.75f;
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    float nextFire;
    public GameObject bulletFiringEffect;
    public AudioClip playerShootingAudio;

    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    
    void FixedUpdate()
    {
        if(!photonView.IsMine)
            return;

        Move();
        if (Input.GetKey(KeyCode.Space))
            photonView.RPC("Fire", RpcTarget.AllViaServer);
    }

    void Move()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            return;

        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        var rotation = Quaternion.LookRotation(new Vector3(horizontalInput,0,verticalInput));
        transform.rotation = rotation;

        Vector3 movementDir = transform.forward * Time.deltaTime * movementSpeed;
        rigidbody.MovePosition(rigidbody.position + movementDir);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            MultiplayerBulletController bullet = collision.gameObject.GetComponent<MultiplayerBulletController>();
            TakeDamage(bullet);
        }
    }

    void TakeDamage(MultiplayerBulletController bullet)
    {
        health -= bullet.damage;
        healthBar.value = health;
        if (health <= 0)
        {
            bullet.owner.AddScore(1); 
            PlayerDied();
        }
    }

    void PlayerDied()
    {
        health = 100;
        healthBar.value = health;
    }

    [PunRPC]
    void Fire() 
    {
        if (Time.time > nextFire) 
        {
            nextFire = Time.time + fireRate;

            GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);
            bullet.GetComponent<MultiplayerBulletController>()?.InitializeBullet(transform.rotation * Vector3.forward, photonView.Owner);

            AudioManager.Instance.Play3D(playerShootingAudio, transform.position);
            VFXManager.Instance.PlayVFX(bulletFiringEffect, bulletPosition.position);

        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (int)stream.ReceiveNext();
            healthBar.value = health;
        }
    }

}
