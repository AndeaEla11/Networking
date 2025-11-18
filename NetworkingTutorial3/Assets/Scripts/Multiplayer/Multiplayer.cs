using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class Multiplayer : MonoBehaviour, IPunObservable
{
    public float movementSpeed = 10f;

    [HideInInspector]
    public int health = 100;
    public Slider healthBar;

    public float fireRate = 0.75f;
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    public GameObject bulletFiringEffect;
    public AudioClip playerShootingAudio;

    float nextFire;

    Rigidbody rb;
    PhotonView photonView;

    public static Multiplayer localPlayer;

    public GameObject nameUIPrefab;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    void Start()
    {

        if (photonView.IsMine)
        {
            var cam = Camera.main.GetComponent<CameraTracking>();
            cam.SetPlayer(transform);
        }

        CreateNameUI();
    }

    void CreateNameUI()
    {
        if (nameUIPrefab == null)
            return;

        GameObject ui = Instantiate(nameUIPrefab);

        var uiScript = ui.GetComponent<PlayerNameUI>();
        if (uiScript != null)
        {
            uiScript.nameText.text = photonView.Owner.NickName;
            uiScript.target = transform;
        }
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
        rb.MovePosition(rb.position + movementDir);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (photonView == null)
            return; 

        if (!photonView.IsMine)
            return;

        if (collision.gameObject.CompareTag("Bullet"))
        {
            var bullet = collision.gameObject.GetComponent<MultiplayerBulletController>();
            if (bullet == null)
                return;

            TakeDamage(bullet);
        }
    }

    void TakeDamage(MultiplayerBulletController bullet)
    {
        health -= bullet.damage;
        healthBar.value = health;
        if (health <= 0)
        {
            if (bullet.owner != null)
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
        if (Time.time <= nextFire)
            return;

        nextFire = Time.time + fireRate;

        GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);

        var bulletCtrl = bullet.GetComponent<MultiplayerBulletController>();
        bulletCtrl?.InitializeBullet(transform.rotation * Vector3.forward, photonView.Owner);

        if (bullet.TryGetComponent(out Collider bulletCol))
        {
            var playerColliders = GetComponentsInChildren<Collider>();
            foreach (var col in playerColliders)
            {
                Physics.IgnoreCollision(bulletCol, col);
            }
        }

        AudioManager.Instance.Play3D(playerShootingAudio, transform.position);
        VFXManager.Instance.PlayVFX(bulletFiringEffect, bulletPosition.position);
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
