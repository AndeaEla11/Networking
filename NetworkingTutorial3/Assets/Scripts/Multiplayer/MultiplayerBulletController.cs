using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts; 


public class MultiplayerBulletController : MonoBehaviourPunCallbacks
{

    Rigidbody rigidBody;
    public float bulletSpeed = 15f;
    public AudioClip BulletHitAudio;
    public GameObject bulletImpactEffect;

    public int damage = 10;

    [HideInInspector]
    public Photon.Realtime.Player owner; 


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        if (rigidBody != null)
            print("Rigidbody is found!");
        else
            print("Rigidbody isn't found!");
    }

    public void InitializeBullet(Vector3 originalDirection, Photon.Realtime.Player givenPlayer) 
    {
        transform.forward = originalDirection;
        rigidBody.linearVelocity = transform.forward * bulletSpeed;

        owner = givenPlayer; 
    }

    private void OnCollisionEnter(Collision collision)
    {

        AudioManager.Instance.Play3D(BulletHitAudio, transform.position);
        VFXManager.Instance.PlayVFX(bulletImpactEffect, transform.position);

        Destroy(gameObject);
    }
}
