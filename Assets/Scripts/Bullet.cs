using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.Networking.Types;

public class Bullet : NetworkBehaviour
{
    public float destroyAfter = 5;
    public Rigidbody2D rb2D;
    public float force = 50;
    public int damage = 50; 
    [SyncVar]
    public string teamName;

    [SyncVar] 
    public uint playerWhoShotId;

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), destroyAfter);
    }

    private void Start()
    {
        Debug.Log("Bullets netId from shooter: " + playerWhoShotId);
        CmdChangeColor();
        //rb2D.AddForce(transform.forward*force);
    }

    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Command(requiresAuthority = false)]
    void CmdChangeColor()
    {
        ChangeColor();
    }
    [ClientRpc]
    void ChangeColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = teamName == "BlueTeam" ? Color.blue : Color.red;
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (player != null && player.teamName != teamName)
        {
            DestroySelf();
        }
        else if (collision.CompareTag("Wall"))
        {
            DestroySelf();
        }
        else if(bullet != null && bullet.teamName != teamName)
            DestroySelf();
        else if (collision.CompareTag("Enemy"))
        {
            DestroySelf();
        }
              


    }
}
