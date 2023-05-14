using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class Bullet : NetworkBehaviour
{
    public float destroyAfter = 5;
    public Rigidbody2D rb2D;
    public float force;
    public int damage;
    public string teamName;
    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), destroyAfter);
    }

    private void Start()
    {
        rb2D.AddForce(transform.forward*force);
    }

    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider co)
    {
        PlayerController player = co.GetComponent<PlayerController>();
        Bullet bullet = co.GetComponent<Bullet>();
        if (player != null && tag.ToString() != player.teamName)
        {
            DestroySelf();
        }else if (bullet.teamName != teamName)
        {
            DestroySelf();
        }
    }
}
