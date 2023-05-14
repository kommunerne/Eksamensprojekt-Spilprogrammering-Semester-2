using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 

public class GunnerNetworkManager : NetworkManager
{

    public GameObject bigGunTank;
    public GameObject gunnerTank;
    public GameObject sniperTank;
    public GameObject machineTank;

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
            characterMessage = new CreateGunnerMessage
            {
                // Johannes do ur magic in here thank you <33
                name = menuScene.GetLoadPlayerName(),
                pinCode = menuScene.GetLoadPinCode(), 
                prefabSelector = menuScene._toggleToInt
            };
        }
        NetworkClient.Send(characterMessage);
    }

    void OnCreateCharacter(NetworkConnectionToClient conn, CreateGunnerMessage message)
    {
        GameObject gameobject;

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
        player.playerName = message.name;
        player.pinCode = message.pinCode;
        player.teamName = bluePlayers <= redPlayers ? "BlueTeam" : "RedTeam";
        NetworkServer.AddPlayerForConnection(conn, gameobject);
    }

}

