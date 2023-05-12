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
    public ArrayList enemySpawnPoints; 

    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<CreateGunnerMessage>(onCreateCharacter);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        CreateGunnerMessage characterMessage = new CreateGunnerMessage
        {
            // Johannes do ur magic in here thank you <33

        };

        NetworkClient.Send(CharacterMessage);
    }

    void onCreateCharacter(NetworkConnectionToClient conn, CreateGunnerMessage message)
    {
        GameObject gameobject;

        Vector3 spawnPoint;

       

        if (bluePlayers >= redPlayers)
        {
            spawnPoint = blueSpawnPoints[Random.Range(0, 2)].transform.position;
        }

        else
        {
            spawnPoint = redSpawnPoints[Random.Range(0, 2)].transform.position;
        }

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
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

