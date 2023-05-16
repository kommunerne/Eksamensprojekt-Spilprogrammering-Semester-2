using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : NetworkBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField] 
    private string detectionTag = "Player";
    [SerializeField] 
    private int radius;
    [SerializeField] 
    private LayerMask playerLayer;

    private ArrayList playersDetected = new ArrayList();
    [SerializeField]
    [SyncVar]
    private Transform playerToAttack;
    
    [SyncVar] public int health;
    
    [SyncVar] public int moveSpeed;
    
    public int exp;

    private GunnerNetworkManager manager;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        manager = FindObjectOfType<GunnerNetworkManager>();
        agent.speed = moveSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        CheckForEnemies();
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
                uint playerWhoShot = bullet.playerWhoShotId;
                GetExp(exp,playerWhoShot);
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
                uint playerWhoShoot = player.netId;
                GetExp(exp,playerWhoShoot);
                //manager.enemyCounter--;
                NetworkServer.Destroy(gameObject);
            }
        }
    }

    [Client]
    void GetExp(int giveExp,uint playerNetId)
    {
        GameObject localPlayer = NetworkServer.spawned[playerNetId].gameObject;
        PlayerController player = localPlayer.GetComponent<PlayerController>();
        Debug.Log("GetExp on enemy called and the player found by this methods has the following netId: "+ player.netId);
        CmdGiveExp(giveExp,player,playerNetId);
    }

    [Command(requiresAuthority = false)]
    void CmdGiveExp(int giveExp,PlayerController player,uint playerNetId)
    {
        Debug.Log("CmdGiveExp on enemy was called");
        if(player.netId==playerNetId)
            player.GivePlayerExp(giveExp);
    }

    [Client]
    void CheckForEnemies()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, radius,playerLayer);

        foreach (var col in enemiesInRange)
        {
            enemiesInRange.OrderBy(hit => Vector3.Distance(hit.transform.position, transform.position));
        }
        
        if (enemiesInRange.Length>0)
        {
            Debug.Log("CmdMoveTowards has been called");
            playerToAttack = enemiesInRange[0].GetComponent<Transform>();
            CmdMoveTowards(playerToAttack);
        }
    }

    [Command(requiresAuthority = false)]
    void CmdMoveTowards(Transform player)
    {
        Debug.Log("CmdMoveTowards has been executed");
        RpcMoveAgent(player);
    }

    [ClientRpc]
    void RpcMoveAgent(Transform player)
    {
        Debug.Log("RpcMoveAgent has been executed");
        agent.destination = player.position;
    }
}
