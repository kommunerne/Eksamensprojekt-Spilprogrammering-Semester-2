using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : NetworkBehaviour
{
    
    [SyncVar] public int health;
    [SyncVar] public int moveSpeed;
    public int exp;
    public NavMeshAgent agent;

    private GunnerNetworkManager manager;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        manager = FindObjectOfType<GunnerNetworkManager>();
        agent.speed = moveSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        PlayerController player = other.GetComponent<PlayerController>();
        if (bullet != null)
        {
            if (health <= bullet.damage)
            {
                CmdGetExp(exp);
                //manager.enemyCounter--;
                NetworkServer.Destroy(gameObject);
            }

            health -= bullet.damage;
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

    [Command(requiresAuthority = false)]
    void CmdGetExp(int giveExp)
    {
        Debug.Log("CmdGetExp on enemy called");
        RpcGiveExp(giveExp);
    }

    [ClientCallback]
    void RpcGiveExp(int giveExp)
    {
        Debug.Log("RcpGiveExp on enemy called");
        GameObject localPlayer = NetworkClient.localPlayer.gameObject;
        PlayerController player = localPlayer.GetComponent<PlayerController>();

        player.ReciveExp(giveExp);
    }
}
