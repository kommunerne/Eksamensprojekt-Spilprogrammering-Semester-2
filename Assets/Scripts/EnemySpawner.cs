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
    public float interval = 6f;
    public GameObject[] spawnPoints; 

    // Start is called before the first frame update
    void Start()
    {
         
    }


    private void GetSpawn()
    {
        Debug.Log("i have been called");
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            Debug.Log("Yassssss");
            spawnPoints = new[] {
            GameObject.Find("EnemySpawn01"), GameObject.Find("EnemySpawn02"), GameObject.Find("EnemySpawn03"), GameObject.Find("EnemySpawn04"), GameObject.Find("EnemySpawn05"),
            GameObject.Find("EnemySpawn06"), GameObject.Find("EnemySpawn07"), GameObject.Find("EnemySpawn08"), GameObject.Find("EnemySpawn09"), GameObject.Find("EnemySpawn10"),
            GameObject.Find("EnemySpawn11"), GameObject.Find("EnemySpawn12"), GameObject.Find("EnemySpawn13"), GameObject.Find("EnemySpawn14"), GameObject.Find("EnemySpawn15"),
            GameObject.Find("EnemySpawn16"), GameObject.Find("EnemySpawn17"), GameObject.Find("EnemySpawn18"), GameObject.Find("EnemySpawn19"), GameObject.Find("EnemySpawn20")
        };
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdSpawnEnemies()
    {
        if (interval <= 0f && SceneManager.GetActiveScene().name == "GameScene")
        {
            Debug.Log("slay");
            GameObject smallEnemy1 = Instantiate(smallEnemy, spawnPoints[Random.Range(0, 20)].transform.position, new Quaternion(0, 0, 0, 0));
            Debug.Log(smallEnemy1);
            Debug.Log(smallEnemy1.transform.position);
            NetworkServer.Spawn(smallEnemy1);


            GameObject smallEnemy2 = Instantiate(smallEnemy, spawnPoints[Random.Range(0, 20)].transform.position, new Quaternion(0, 0, 0, 0));
            Debug.Log(smallEnemy2);
            Debug.Log(smallEnemy2.transform.position);
            NetworkServer.Spawn(smallEnemy2);



            GameObject smallEnemy3 = Instantiate(smallEnemy, spawnPoints[Random.Range(0, 20)].transform.position, new Quaternion(0, 0, 0, 0));
            Debug.Log(smallEnemy3);
            Debug.Log(smallEnemy3.transform.position);
            NetworkServer.Spawn(smallEnemy3);


            enemyCounter += 3;
            interval = 6f;
            Debug.Log("Enemy spawn:" + enemyCounter);

        }
        else
        {
            interval -= Time.deltaTime;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer) {
            Debug.Log(interval);
        GetSpawn();
        CmdSpawnEnemies();
        }
    }
    
    [Client]
    public void callSpawnEnemy()
    {
        CmdSpawnEnemies();
    }
    
}
