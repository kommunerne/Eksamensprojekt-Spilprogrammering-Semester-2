using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject smallEnemy;
    public GameObject mediumEnemy;
    public GameObject largeEnemy;
    public GameObject bossEnemy;

    public int mediumEnemyCount = 0;
    public int largeEnemyCount = 0;
    public int enemyCounter = 0;
    public float interval = 0f;
    public ArrayList spawnPoints = new ArrayList();
    public Transform[] spawnPointsTransform;
    public int randomNumber;
    private bool sceneChanged = false;

    
    void Update()
    {
        if (isServer) {
            if (spawnPoints.Count <= 0 && SceneManager.GetActiveScene().name == "GameScene")
                sceneChanged = true;
            GetSpawn();
            SpawnSmallEnemies();
        }
    }
    [Client]
    private void GetSpawn()
    {
        if (SceneManager.GetActiveScene().name == "GameScene" && sceneChanged)
        {
            Debug.Log("Spawn Points have been set");
            spawnPointsTransform = new [] {
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
            foreach (var point in spawnPointsTransform)
            {
                spawnPoints.Add(point);
            }
            sceneChanged = false;
        }
    }
    
    [Client]
    void SpawnSmallEnemies()
    {
        if (interval <= 0f && SceneManager.GetActiveScene().name == "GameScene")
        {
            mediumEnemyCount++;
            largeEnemyCount++;
            enemyCounter += 1;
            interval = 3f;
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
        if (spawnPoints.Count > 0)
        {
            Debug.Log("Spawn Enemy");
            randomNumber = Random.Range(0, spawnPoints.Count-1);
            Transform enemyTransform = (Transform)spawnPoints[randomNumber];
            GameObject smallEnemy1 = Instantiate(smallEnemy, enemyTransform.position, enemyTransform.rotation);
            NetworkServer.Spawn(smallEnemy1);
            spawnPoints.Remove(enemyTransform);
            Debug.Log(spawnPoints.Count);

            if (mediumEnemyCount==3)
            {
                randomNumber = Random.Range(0, spawnPoints.Count-1);
                Transform enemyTransform2 = (Transform)spawnPoints[randomNumber];
                GameObject mediumEnemy1 = Instantiate(mediumEnemy, enemyTransform2.position, enemyTransform2.rotation);
                NetworkServer.Spawn(mediumEnemy1);
                spawnPoints.Remove(enemyTransform2);
                Debug.Log(spawnPoints.Count);
                mediumEnemyCount = 0;
            }
            if (largeEnemyCount==5)
            {
                randomNumber = Random.Range(0, spawnPoints.Count-1);
                Transform enemyTransform3 = (Transform)spawnPoints[randomNumber];
                GameObject largeEnemy1 = Instantiate(largeEnemy, enemyTransform3.position, enemyTransform3.rotation);
                NetworkServer.Spawn(largeEnemy1);
                spawnPoints.Remove(enemyTransform3);
                Debug.Log(spawnPoints.Count);
                largeEnemyCount = 0;
            }
        }
    }
}
