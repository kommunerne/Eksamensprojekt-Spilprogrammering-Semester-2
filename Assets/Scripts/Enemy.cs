using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    
    [SyncVar] public int health;
    [SyncVar] public int moveSpeed;
    public int exp;

    private GunnerNetworkManager manager;
    
    // Start is called before the first frame update
    void Start()
    {
       manager = FindObjectOfType<GunnerNetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        PlayerController player = other.GetComponent<PlayerController>();
        if (bullet != null)
        {
            if (health < bullet.damage)
            {
                CmdGetExp(exp);
                //manager.enemyCounter--;
                NetworkServer.Destroy(gameObject);
            }

            health = health - bullet.damage;
        }

        if (player != null)
        {
            if (player.currentHp < health)
            {
                health = health - player.currentHp;
            }
            else
            {
                CmdGetExp(exp);
                //manager.enemyCounter--;
                NetworkServer.Destroy(gameObject);
            }
        }
    }

    [Command]
    void CmdGetExp(int exp)
    {
        RpcGiveExp(exp);
    }

    [ClientRpc]
    void RpcGiveExp(int exp)
    {
        GameObject localPlayer = NetworkClient.localPlayer.gameObject;
        PlayerController player = localPlayer.GetComponent<PlayerController>();

        player.exp += exp;
    }
}
