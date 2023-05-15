using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;
using Random = UnityEngine.Random;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject smallEnemy;
    public GameObject mediumEnemy;
    public GameObject largeEnemy;
    public GameObject bossEnemy;

    public int enemyCounter = 0;
    public float interval = 0f;
    public Transform[] spawnPoints;

    public int randomNumber;
    // Update is called once per frame
    void Update()
    {
        if (isServer) {
            GetSpawn();
            SpawnSmallEnemies();
        }
    }
    [Client]
    private void GetSpawn()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            spawnPoints = new[] {
                GameObject.Find("EnemySpawn01").GetComponent<Transform>(), GameObject.Find("EnemySpawn02").GetComponent<Transform>(), 
                GameObject.Find("EnemySpawn03").GetComponent<Transform>(), GameObject.Find("EnemySpawn04").GetComponent<Transform>(), 
                GameObject.Find("EnemySpawn05").GetComponent<Transform>(), GameObject.Find("EnemySpawn06").GetComponent<Transform>(), 
                GameObject.Find("EnemySpawn07").GetComponent<Transform>(), GameObject.Find("EnemySpawn08").GetComponent<Transform>(), 
                GameObject.Find("EnemySpawn09").GetComponent<Transform>(), GameObject.Find("EnemySpawn10").GetComponent<Transform>(), 
                GameObject.Find("EnemySpawn11").GetComponent<Transform>(), GameObject.Find("EnemySpawn12").GetComponent<Transform>(), 
                GameObject.Find("EnemySpawn13").GetComponent<Transform>(), GameObject.Find("EnemySpawn14").GetComponent<Transform>(), 
                GameObject.Find("EnemySpawn15").GetComponent<Transform>(), GameObject.Find("EnemySpawn16").GetComponent<Transform>(), 
                GameObject.Find("EnemySpawn17").GetComponent<Transform>(), GameObject.Find("EnemySpawn18").GetComponent<Transform>(), 
                GameObject.Find("EnemySpawn19").GetComponent<Transform>(), GameObject.Find("EnemySpawn20").GetComponent<Transform>()
            };
        }
    }
    
    [Client]
    void SpawnSmallEnemies()
    {
        if (interval <= 0f && SceneManager.GetActiveScene().name == "GameScene")
        {
            enemyCounter += 3;
            interval = 6f;
            CmdSpawnEnemies();
        }
        else
        {
            interval -= Time.deltaTime;
        }
    }
 
    [Command(requiresAuthority = false)]
    void CmdSpawnEnemies()
    {
        randomNumber = Random.Range(0, 19);
        GameObject smallEnemy1 = Instantiate(smallEnemy, spawnPoints[randomNumber].position, spawnPoints[randomNumber].rotation);
        NetworkServer.Spawn(smallEnemy1);

        randomNumber = Random.Range(0, 19);
        GameObject smallEnemy2 = Instantiate(smallEnemy, spawnPoints[randomNumber].position, spawnPoints[randomNumber].rotation);
        NetworkServer.Spawn(smallEnemy2);
            
        randomNumber = Random.Range(0, 19);
        GameObject smallEnemy3 = Instantiate(smallEnemy, spawnPoints[randomNumber].position, spawnPoints[randomNumber].rotation);
        NetworkServer.Spawn(smallEnemy3);


    }
}
