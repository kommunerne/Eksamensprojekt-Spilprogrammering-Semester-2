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
    public float bonusFireRatePerLevel = 1.0f;
    public float bonusMoveSpeed = 5f;
    public int bonusHpRegenPerLevel = 5;

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
    
    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        uiController = GetComponent<PlayerUIController>();
        playerGotHit = false;
        nextFireTime = fireRate;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            Shoot();
            RotateToMouse2D();
            CharacterMovement();
            RegenHealthOverTime();
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
    
        // Main Methods
        //[ClientRpc]
        void RotateToMouse2D()
        {
            Vector3 mousePosition = cameraToUse.ScreenToWorldPoint(Input.mousePosition);

            // Calculate the angle between the mouse and the player's position
            Vector3 direction = mousePosition - transform.position;
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
        
        private void CharacterMovement()
        {
            if (Input.GetKey(KeyCode.A))
                _rb2D.AddForce(Vector2.left * moveSpeed);
            if (Input.GetKey(KeyCode.D))
                _rb2D.AddForce(Vector2.right * moveSpeed);
            if (Input.GetKey(KeyCode.W))
                _rb2D.AddForce(Vector2.up * moveSpeed);
            if (Input.GetKey(KeyCode.S))
                _rb2D.AddForce(Vector2.down * moveSpeed);
        }


        [Client]
        void Shoot()
        {
            if (Input.GetButtonDown("Fire1") && nextFireTime <= 0)
            {
                
                nextFireTime = fireRate;
                CmdShoot();
            }
            else
            {
                nextFireTime -= Time.deltaTime;
            }
        }
        // Makes the player shoot
        [Command] 
        private void CmdShoot()
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.damage = dmg;
            bullet.tag = teamName;
            if (teamName == "RedTeam")
                spriteRenderer.color = Color.red;
            else if (teamName == "BlueTeam")
                spriteRenderer.color = Color.blue;
                
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
            fireRate += bonusFireRatePerLevel;
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
            hpRegen += bonusHpPerLevel;
            statPoints--;
        }
    
        
        // ***********************************
        //
        // Denne metode bliver slettet senere. Den er lavet for at kunne teste om "kill" mechanicen fungere.
        // Jeg skal nok slette den når vi kommer dertil.
        //
        // ***********************************
        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            Bullet bullet = other.GetComponent<Bullet>();
            Enemy enemy = other.GetComponent<Enemy>();
            if (bullet != null)
            {
                
                playerGotHit = true;
                if (currentHp <= bullet.damage)
                {
                    CmdPlayerDied();
                }
                currentHp = currentHp - bullet.damage;
                Debug.Log(currentHp);
                Invoke(nameof(HitReset), 3);
            } 
            if (enemy != null)
            {
                if (currentHp < enemy.health)
                {
                    CmdPlayerDied();
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
        
        void HitReset()
        {
            playerGotHit = false;
        }

        [Command]
        void CmdPlayerDied()
        {
            PlayerDead();
        }

        [ClientRpc]
        void PlayerDead()
        {
            _playerDead = true;
            uiController.DeathScreen();
            gameObjectToBeRotated.layer = 8;
            barrelOfTheTank.layer = 8;
            _rb2D.velocity = new Vector2(0, 0);
        }
}
