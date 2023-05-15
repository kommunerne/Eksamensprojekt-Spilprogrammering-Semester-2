using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GunnerNetworkManager : NetworkManager
{

    public GameObject bigGunTank;
    public GameObject gunnerTank;
    public GameObject sniperTank;
    public GameObject machineTank;
    public GameObject smallEnemy;
    public uint smallEnemyID = 1832388894;
    public GameObject mediumEnemy;
    public GameObject largeEnemy;
    public GameObject bossEnemy;
    public int enemyCounter = 0; 
    public float interval = 6f; // Noget galt med hvordan den her virker med smallEnemySpawn --> Kigger p� det soon 

    public int redPlayers;
    public int bluePlayers; 
    public Transform[] redSpawnPoints;
    public Transform[] blueSpawnPoints;
    public Transform[] enemySpawnPoints;
    public Transform[] bossSpawnPoint;
    
    public override void OnStartServer()
    {
        base.OnStartServer();
        
        NetworkServer.RegisterHandler<CreateGunnerMessage>(OnCreateCharacter);
      //  NetworkClient.RegisterPrefab(smallEnemy, smallEnemyID);
    //    NetworkClient.RegisterSpawnHandler(smallEnemyID, smallEnemySpawn(), null);

    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        CreateGunnerMessage characterMessage;
        MainMenuController menuScene = GetComponent<MainMenuController>();
        
        if(menuScene.isNewPlayer)
        {
            characterMessage = new CreateGunnerMessage
            {
                // Johannes do ur magic in here thank you <33
                name = menuScene.GetNewPlayerName(),
                pinCode = menuScene.GetNewPinCode(), 
                prefabSelector = menuScene._toggleToInt
            };
        } 
        else
        {
            characterMessage = new CreateGunnerMessage {
                name = menuScene.GetLoadPlayerName(),
                pinCode = menuScene.GetLoadPinCode(),
                prefabSelector = menuScene.loadedPlayer.prefabNr,
                level = menuScene.loadedPlayer.level,
                exp = menuScene.loadedPlayer.exp,
                score = menuScene.loadedPlayer.score
                
            };
        }
        NetworkClient.Send(characterMessage);
    }

    void OnCreateCharacter(NetworkConnectionToClient conn, CreateGunnerMessage message)
    {
        GameObject gameobject;
        DBScript _db = GetComponent<DBScript>();
        MainMenuController menuScene = GetComponent<MainMenuController>();
        // Getting a random spawn for the player depending on which team he should spawn in on.
        GameObject[] redSpawns =
        {
            GameObject.Find("SpawnR01"), GameObject.Find("SpawnR02"), GameObject.Find("SpawnR03"),GameObject.Find("SpawnR04"),GameObject.Find("SpawnR05"),GameObject.Find("SpawnR06")
        };
        GameObject[] blueSpawns = {
            GameObject.Find("SpawnB01"), GameObject.Find("SpawnB02"), GameObject.Find("SpawnB03"), GameObject.Find("SpawnB04"), GameObject.Find("SpawnB05"), GameObject.Find("SpawnB06")
        };

        var spawnPoint = bluePlayers <= redPlayers ? blueSpawns[Random.Range(0, 5)].transform.position : redSpawns[Random.Range(0, 5)].transform.position;


        if(message.prefabSelector == 1)
        {
            gameobject = Instantiate(bigGunTank, spawnPoint, new Quaternion(0,0,0,0));
        }

        else if (message.prefabSelector == 2)
        {
            gameobject = Instantiate(gunnerTank, spawnPoint, new Quaternion(0, 0, 0, 0));
        }

        else if (message.prefabSelector == 3)
        {
            gameobject = Instantiate(sniperTank, spawnPoint, new Quaternion(0, 0, 0, 0));
        }

        else if (message.prefabSelector == 4)
        {
            gameobject = Instantiate(machineTank, spawnPoint, new Quaternion(0, 0, 0, 0));
        }

        else
        {
            gameobject = Instantiate(gunnerTank, spawnPoint, new Quaternion(0, 0, 0, 0));
            
        }
        PlayerController player = gameobject.GetComponent<PlayerController>();
        
        if (menuScene.isNewPlayer)
        {
            player.playerName = message.name;
            player.pinCode = message.pinCode;
            player.prefabNr = message.prefabSelector;
        }else
        {
            Player loadPlayer = _db.GetPlayer(message.name, message.pinCode);
            player.playerName = loadPlayer.username;
            player.pinCode = loadPlayer.pinCode;
            player.prefabNr = loadPlayer.prefabNr;
            player.level = loadPlayer.level;
            player.exp = loadPlayer.exp;
            player.score = loadPlayer.score;
        }
        player.teamName = bluePlayers <= redPlayers ? "BlueTeam" : "RedTeam";
        if (player.teamName == "BlueTeam")
            bluePlayers++;
        else if (player.teamName == "RedTeam")
        {
            redPlayers++;
        }
        NetworkServer.AddPlayerForConnection(conn, gameobject);
    }

 /*   public SpawnDelegate smallEnemySpawn()
    {

        GameObject[] enemySpawns = {
            GameObject.Find("EnemySpawn01"), GameObject.Find("EnemySpawn02"), GameObject.Find("EnemySpawn03"), GameObject.Find("EnemySpawn04"), GameObject.Find("EnemySpawn05"), 
            GameObject.Find("EnemySpawn06"), GameObject.Find("EnemySpawn07"), GameObject.Find("EnemySpawn08"), GameObject.Find("EnemySpawn09"), GameObject.Find("EnemySpawn10"), 
            GameObject.Find("EnemySpawn11"), GameObject.Find("EnemySpawn12"), GameObject.Find("EnemySpawn13"), GameObject.Find("EnemySpawn14"), GameObject.Find("EnemySpawn15"), 
            GameObject.Find("EnemySpawn16"), GameObject.Find("EnemySpawn17"), GameObject.Find("EnemySpawn18"), GameObject.Find("EnemySpawn19"), GameObject.Find("EnemySpawn20")
        };

        if (interval == 0f)
        {
           
           GameObject smallEnemy1 = Instantiate(smallEnemy, enemySpawns[Random.Range(0,20)].transform.position, new Quaternion(0,0,0,0));
            Debug.Log(smallEnemy1);
            Debug.Log(smallEnemy1.transform.position);
            NetworkServer.Spawn(smallEnemy1);
            

            GameObject smallEnemy2 = Instantiate(smallEnemy, enemySpawns[Random.Range(0, 20)].transform.position, new Quaternion(0, 0, 0, 0));
            Debug.Log(smallEnemy2);
            Debug.Log(smallEnemy2.transform.position);
            NetworkServer.Spawn(smallEnemy2);
            


            GameObject smallEnemy3 = Instantiate(smallEnemy, enemySpawns[Random.Range(0, 20)].transform.position, new Quaternion(0, 0, 0, 0));
            Debug.Log(smallEnemy3);
            Debug.Log(smallEnemy3.transform.position); 
            NetworkServer.Spawn(smallEnemy3);
            

            enemyCounter += 3;
            interval = 6f;
            Debug.Log("Enemy spawn:" + enemyCounter);
            return smallEnemySpawn();

        }
        else
        {
            interval -= Time.deltaTime;
            return smallEnemySpawn();
        }
        
    }

    public void mediumEnemySpawn()
    {

        GameObject[] enemySpawns = {
            GameObject.Find("EnemySpawn1"), GameObject.Find("EnemySpawn2"), GameObject.Find("EnemySpawn3"), GameObject.Find("EnemySpawn4"), GameObject.Find("EnemySpawn5"),
            GameObject.Find("EnemySpawn 6"), GameObject.Find("EnemySpawn7"), GameObject.Find("EnemySpawn8"), GameObject.Find("EnemySpawn9"), GameObject.Find("EnemySpawn10"),
            GameObject.Find("EnemySpawn11"), GameObject.Find("EnemySpawn12"), GameObject.Find("EnemySpawn13"), GameObject.Find("EnemySpawn14"), GameObject.Find("EnemySpawn15"),
            GameObject.Find("EnemySpawn16"), GameObject.Find("EnemySpawn17"), GameObject.Find("EnemySpawn18"), GameObject.Find("EnemySpawn19"), GameObject.Find("EnemySpawn20")
        };

        if (interval == 0f)
        {
            GameObject mediumEnemy1 = Instantiate(mediumEnemy, enemySpawns[Random.Range(0, 20)].transform.position, new Quaternion(0, 0, 0, 0));
            NetworkServer.Spawn(mediumEnemy1);

            GameObject mediumEnemy2 = Instantiate(mediumEnemy, enemySpawns[Random.Range(0, 20)].transform.position, new Quaternion(0, 0, 0, 0));
            NetworkServer.Spawn(mediumEnemy2);

            GameObject mediumEnemy3 = Instantiate(mediumEnemy, enemySpawns[Random.Range(0, 20)].transform.position, new Quaternion(0, 0, 0, 0));
            NetworkServer.Spawn(mediumEnemy3);

            enemyCounter += 3;
            interval = 6f;

        }
        else
        {
            interval -= Time.deltaTime;
        }

    }

    public void largeEnemySpawn()
    {

        GameObject[] enemySpawns = {
            GameObject.Find("EnemySpawn1"), GameObject.Find("EnemySpawn2"), GameObject.Find("EnemySpawn3"), GameObject.Find("EnemySpawn4"), GameObject.Find("EnemySpawn5"),
            GameObject.Find("EnemySpawn 6"), GameObject.Find("EnemySpawn7"), GameObject.Find("EnemySpawn8"), GameObject.Find("EnemySpawn9"), GameObject.Find("EnemySpawn10"),
            GameObject.Find("EnemySpawn11"), GameObject.Find("EnemySpawn12"), GameObject.Find("EnemySpawn13"), GameObject.Find("EnemySpawn14"), GameObject.Find("EnemySpawn15"),
            GameObject.Find("EnemySpawn16"), GameObject.Find("EnemySpawn17"), GameObject.Find("EnemySpawn18"), GameObject.Find("EnemySpawn19"), GameObject.Find("EnemySpawn20")
        };

        if (interval == 0f)
        {
            GameObject largeEnemy1 = Instantiate(largeEnemy, enemySpawns[Random.Range(0, 20)].transform.position, new Quaternion(0, 0, 0, 0));
            NetworkServer.Spawn(largeEnemy1);
            Debug.Log(largeEnemy1);

            GameObject largeEnemy2 = Instantiate(largeEnemy, enemySpawns[Random.Range(0, 20)].transform.position, new Quaternion(0, 0, 0, 0));
            NetworkServer.Spawn(largeEnemy2);
            Debug.Log(largeEnemy2);

            GameObject largeEnemy3 = Instantiate(largeEnemy, enemySpawns[Random.Range(0, 20)].transform.position, new Quaternion(0, 0, 0, 0));
            NetworkServer.Spawn(largeEnemy3);
            Debug.Log(largeEnemy3);

            enemyCounter += 3;
            interval = 6f;

        }
        else
        {
            interval -= Time.deltaTime;
        }

    }

       public override void Update()
    {
        Debug.Log(enemyCounter);
        base.Update();
        if (enemyCounter <= 30 || enemyCounter == 0 &&  SceneManager.GetActiveScene().ToString() == "GameScene")
        {
            smallEnemySpawn(); // --> the fuck is going wrong lmao
            Debug.Log("HELLO!");
        }
        
        
    } */

}

   


