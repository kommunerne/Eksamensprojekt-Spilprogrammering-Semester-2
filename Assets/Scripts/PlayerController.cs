using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    // variables
    //public GameObject player;
    
    [Header("Cameras")]
    public Camera cameraToUse;
    public Camera miniMap;
    public Camera deathScreen;
    
    [Header("GameObject to follow the mouse")]
    public GameObject gameObjectToBeRotated;
    public GameObject barrelOfTheTank;
    
    
    [Header("PlayerUIController Script")]
    [SerializeField] private PlayerUIController uiController;
        
    
    [Header("Player Stats")]
    [SyncVar]
    public int maxHp = 100;
    [SyncVar]
    public int currentHp;
    [SyncVar]
    public int dmg = 20;
    [SyncVar]
    public float moveSpeed = 5f;
    [SyncVar]
    public int hpRegen = 5;
    [SyncVar]
    public int level;
    [SyncVar]
    public int exp;
    [SyncVar]
    public int statPoints;
    [SyncVar]
    public int score;
    [SyncVar] 
    public int prefabNr;
    [SyncVar]
    public float nextFireTime = 0f;
    [SyncVar]
    public float fireRate = 0.5f;
    public float bulletSpeed = 10f;
    [Header("Stats gained pr upgrade")]
    public int bonusHpPerLevel = 100;
    public int bonusDmgPerLevel = 20;
    private float _bonusFireRatePerLevel;
    public float bonusMoveSpeed = 5f;
    public int bonusHpRegenPerLevel = 5;

    [SyncVar] private float _expTilNextLevel = 100;
    private float _expTilNextLevelMultiplier = 1.35f;
    
    [Header("Class Specific Variables")] 
    public float bulletRadius;

    [Header("Player Info")]
    [SyncVar]
    public string playerName;
    [SyncVar]
    public string pinCode;
    [SyncVar]
    public string teamName;
    [SyncVar]
    public bool playerGotHit;
    private bool _playerDead = false;
    private Rigidbody2D _rb2D;

    [Space] 
    [Header("Prefabs & GameObjects:")] 
    public GameObject bulletPrefab;
    public Transform firePoint;
    public SpriteRenderer miniMapIcon;
    
    
    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        uiController = GetComponent<PlayerUIController>();
        playerGotHit = false;
        nextFireTime = fireRate;
        if(isLocalPlayer)
            CmdChangeSpriteColer();
    }

    

    private void Update()
    {
        if (isLocalPlayer && !_playerDead)
        {
            Shoot();
            RotateTank();
            Move();
            RegenHealthOverTime();
            ExpToLevel();
            UpdatePlayerInfo();
            UpdatePlayerStats();
            ShowHud();
            return;
        }
        Debug.Log(transform.name + " is not local :( " + netId);
        cameraToUse.enabled = false;
        miniMap.enabled = false;
        deathScreen.enabled = false;

    }

    private void FixedUpdate()
    {
        
    }
    
    
    // Methods

    [Client]
    void ExpToLevel()
    {
        if (exp >= _expTilNextLevel)
        {
            CmdLevelPlayerUp();
        }
    }

    [Command]
    void CmdLevelPlayerUp()
    {
        exp = 0;
        _expTilNextLevel *= _expTilNextLevelMultiplier;
        statPoints++;
        level++;
    }
    
    
        // Main Methods

        [Client]
        void RotateTank()
        {
            Vector3 mousePosition = cameraToUse.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;
            CmdRotateTank(mousePosition,position);
        }
        
        [Command]
        void CmdRotateTank(Vector3 mousePosition,Vector3 position)
        {
            // Calculate the angle between the mouse and the player's position
            Vector3 direction = mousePosition - position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate the player to face the mouse
            gameObjectToBeRotated.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        
        private void RegenHealthOverTime()
        {
            if (currentHp < maxHp && !playerGotHit)
            {
                currentHp += hpRegen * (int)Time.deltaTime; // Increase current health by regenRate per second
                currentHp = Mathf.Clamp(currentHp, 0, maxHp); // Clamp current health to be within the range of 0 and max health
            }
        }


        [Client]
        void Move()
        {
            if (Input.GetKey(KeyCode.A))
                CmdMove(Vector2.left,moveSpeed);
            if (Input.GetKey(KeyCode.D))
                CmdMove(Vector2.right,moveSpeed);
            if (Input.GetKey(KeyCode.W))
                CmdMove(Vector2.up,moveSpeed);
            if (Input.GetKey(KeyCode.S))
                CmdMove(Vector2.down, moveSpeed);
        }
        
        [Command]
        void CmdMove(Vector2 direction,float playerMoveSpeed)
        {
            _rb2D.AddForce(direction*playerMoveSpeed);
        }
        
        /*[ClientRpc]
        private void RpcMove()
        {
            Debug.Log("The RpcMove was executed");
            if (Input.GetKey(KeyCode.A))
                _rb2D.AddForce(Vector2.left * moveSpeed);
            if (Input.GetKey(KeyCode.D))
                _rb2D.AddForce(Vector2.right * moveSpeed);
            if (Input.GetKey(KeyCode.W))
                _rb2D.AddForce(Vector2.up * moveSpeed);
            if (Input.GetKey(KeyCode.S))
                _rb2D.AddForce(Vector2.down * moveSpeed);
        }*/


        [Client]
        void Shoot()
        {
            if (Input.GetButtonDown("Fire1") && nextFireTime <= 0)
            {
                Debug.Log("The players netId:" + netId);
                nextFireTime = fireRate;
                
                if(teamName == "BlueTeam")
                    CmdShoot(true);
                else if (teamName == "RedTeam")
                    CmdShoot(false);
            }
            else
            {
                nextFireTime -= Time.deltaTime;
            }
        }
        // Makes the player shoot
        [Command] 
        private void CmdShoot(bool isPlayerBlueTeam)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.damage = dmg;
            bulletScript.teamName = isPlayerBlueTeam ? "BlueTeam" : "RedTeam";
            bulletScript.playerWhoShotId = netId;

            // Set the speed of the bullet
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = firePoint.right * bulletSpeed;
            NetworkServer.Spawn(bullet);
        }
        
       /* //[ClientRpc]
        //private void RpcFire()
        {
            // Spawn a bullet at the fire point
               
            
        }
        
        */
        //___________________________________________________

        
        
    // UI Methods
    [Command]
    private void CmdChangeSpriteColer()
    {
        RpcChangePlayerColor();
    }

    [ClientRpc]
    private void RpcChangePlayerColor()
    {
        if(teamName == "BlueTeam")
            miniMapIcon.color = Color.blue;
        else
            miniMapIcon.color = Color.red;
    }

    [Command]
    public void CmdUpgradeHp()
    {
        RpcHpUpgrade();
    }
    
    [Command]
    public void CmdUpgradeDmg()
    {
        RpcDmgUpgrade();
    }
    
    [Command]
    public void CmdUpgradeFireRate()
    {
        RpcFireRateUpgrade();
    }
    
    [Command]
    public void CmdUpgradeMoveSpeed()
    {
        RpcMoveSpeedUpgrade();
    }
    
    [Command]
    public void CmdUpgradeHpRegen()
    {
        RpcHpRegenUpgrade();
    }
    
    
        [ClientRpc]
        void RpcHpUpgrade()
        {
            maxHp += bonusHpPerLevel;
            currentHp += bonusHpPerLevel;
            statPoints--;
        }
        
        [ClientRpc]
        void RpcDmgUpgrade()
        {
            dmg += bonusDmgPerLevel;
            statPoints--;
        }
        
        [ClientRpc]
        void RpcFireRateUpgrade()
        {
            _bonusFireRatePerLevel = fireRate * 0.2f;
            fireRate -= _bonusFireRatePerLevel;
            statPoints--;
        }
        
        [ClientRpc]
        void RpcMoveSpeedUpgrade()
        {
            moveSpeed += bonusMoveSpeed;
            statPoints--;
        }
        
        [ClientRpc]
        void RpcHpRegenUpgrade()
        {
            hpRegen += bonusHpRegenPerLevel;
            statPoints--;
        }
    
        
        // ***********************************
        //
        // Denne metode bliver slettet senere. Den er lavet for at kunne teste om "kill" mechanicen fungere.
        // Jeg skal nok slette den når vi kommer dertil.
        //
        // ***********************************

        [ServerCallback]
        private void OnTriggerEnter2D(Collider2D other)
        {
            Bullet bullet = other.GetComponent<Bullet>();
            Enemy enemy = other.GetComponent<Enemy>();
            if (bullet != null && bullet.teamName != teamName)
            {
                
                playerGotHit = true;
                if (currentHp <= bullet.damage)
                {
                    CmdPlayerDiedByPlayer(bullet.playerWhoShotId);
                }
                currentHp = currentHp - bullet.damage;
                Debug.Log(currentHp);
                Invoke(nameof(HitReset), 3);
            } 
            if (enemy != null)
            {
                if (currentHp < enemy.health)
                {
                    CmdPlayerDiedByEnemy(enemy.name);
                }
                else
                {
                    playerGotHit = true;
                    currentHp = currentHp - enemy.health;
                    Debug.Log(currentHp);
                    Invoke(nameof(HitReset), 3);
                }
            }
        }

        [Client]
        public void ReciveExp(int enemyExp)
        {
            Debug.Log("RecieveExp on Player called");
            CmdGetExp(enemyExp);
        }

        [Command]
        void CmdGetExp(int enemyExp)
        {
            Debug.Log("CmdGetExp on Player called");
            exp += enemyExp;
        }
        
        void HitReset()
        {
            playerGotHit = false;
        }

        [Command]
        void CmdPlayerDiedByPlayer(uint enemyPlayer)
        {
            GameObject localPlayer = NetworkServer.spawned[enemyPlayer].gameObject;
            string nameOfKiller = localPlayer.name;
            PlayerDead(nameOfKiller);
        }

        [Command]
        void CmdPlayerDiedByEnemy(string enemyName)
        {
            PlayerDead(enemyName);
        }
        
        [ClientRpc]
        void PlayerDead(string enemyPlayer)
        {
            _playerDead = true;
            uiController.DeathScreen(enemyPlayer);
            gameObjectToBeRotated.layer = 8;
            barrelOfTheTank.layer = 8;
            _rb2D.velocity = new Vector2(0, 0);
        }

        [TargetRpc]
        public void GivePlayerExp(int newExp)
        {
            score += newExp;
            Debug.Log("TargetRpc GivePlayerExp() was called on this netId: "+netId);
            exp += newExp;
        }
        
        
        // UI
        [Client]
        void ShowHud()
        {
            uiController.CmdShowHud();
        }

        [Client]
        void UpdatePlayerInfo()
        {
            uiController.CmdUpdatePlayerInfo();
        }

        [Client]
        void UpdatePlayerStats()
        {
            uiController.CmdUpdatePlayerStats();
        }
        
}
