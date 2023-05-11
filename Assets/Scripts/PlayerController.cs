using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // variables
    
    [Header("Main Camera")]
    public Camera cameraToUse;
    
    [Header("GameObject to follow the mouse")]
    public GameObject gameObjectToBeRotated;
    
    
    [Header("PlayerUIController Script")]
    [SerializeField] private PlayerUIController uiController;
        
    
    [Header("Player Stats")]
    public int maxHp = 100;
    public int currentHp = 100;
    public int dmg = 20;
    public float firerate = 1.0f;
    public float moveSpeed = 5f;
    public int hpregen = 5;
    public int level;
    public int exp;
    public int statPoints;
    public int score;
    
    [Header("Stats gained pr upgrade")]
    public int bonusHpPerLevel = 100;
    public int bonusDmgPerLevel = 20;
    public float bonusFireRatePerLevel = 1.0f;
    public float bonusMoveSpeed = 5f;
    public int bonusHpRegenPerLevel = 5;

    [Header("Class Specific Variables")] 
    public float bulletRadius;

    [Header("Player Info")]
    public string playerName;
    public int pinCode;
    
    public bool playerGotHit;
    private bool _playerDead = false;
    private Rigidbody2D _rb2D;

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        uiController = GetComponent<PlayerUIController>();
        playerGotHit = false;
    }

    private void Update()
    {
        RotateToMouse2D();
        PlayerShoot();
        PlayerTakeDamage();
        CharacterMovement();
        RegenHealtOverTime();
        HaveTakenDamageCheck();
    }

    private void FixedUpdate()
    {
        throw new NotImplementedException();
    }
    
    
    // Methods
    
        // Main Methods
        //*******************************************************
        //
        // Det er disse metoder du skal implementere Erik :) 
        //
        //*******************************************************
        
        // Makes the player follow the mouse ( Use the objectToGetRotated gameobject as the object that should be rotated)
        // If you rotated the entire player objects, you will then also rotate the cameras
        
        void RotateToMouse2D()
        {
            throw new NotImplementedException();
        }
    
        // Makes the player regen health over time
        private void RegenHealtOverTime()
        {
            throw new NotImplementedException();
        }

        // Controls the players movement ( PlayerMovement() er et ugyldigt navn for en metode. Derfor blev det CharacterMovement() )
        // Bare lige FYI :) 
        private void CharacterMovement()
        {
            throw new NotImplementedException();
        }

        // Makes the player take damage if the collider with enemies or enemyteams bullets
        private void PlayerTakeDamage()
        {
            throw new NotImplementedException();
        }

        // Makes the player shoot
        private void PlayerShoot()
        {
            throw new NotImplementedException();
        }

        // Pauses Health Regeneration when hit by an enemy
        private void HaveTakenDamageCheck()
        {
            throw new NotImplementedException();
        }
        
        //___________________________________________________
        
    // UI Methods
        
        public void HpUpgrade()
        {
            maxHp += bonusHpPerLevel;
            currentHp += bonusHpPerLevel;
            statPoints--;
        }

        public void DmgUpgrade()
        {
            dmg += bonusDmgPerLevel;
            statPoints--;
        }

        public void FirerateUpgrade()
        {
            firerate += bonusFireRatePerLevel;
            statPoints--;
        }

        public void MovespeedUpgrade()
        {
            moveSpeed += bonusMoveSpeed;
            statPoints--;
        }

        public void HpregenUpgrade()
        {
            hpregen += bonusHpPerLevel;
            statPoints--;
        }
    
        
        // ***********************************
        //
        // Denne metode bliver slettet senere. Den er lavet for at kunne teste om "kill" mechanicen fungere.
        // Jeg skal nok slette den når vi kommer dertil.
        //
        // ***********************************
        public void Hit()
        {
            if (Input.GetKeyDown(KeyCode.K) && playerGotHit)
            {
                Debug.Log("Dubble Hit");
                CancelInvoke();
                Invoke(nameof(HitReset),3);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                playerGotHit = true;
                if (currentHp <= 20)
                {
                    _playerDead = true;
                    uiController.DeathScreen();
                    gameObjectToBeRotated.layer = 8;
                    _rb2D.velocity = new Vector2(0, 0);
                }
                currentHp = currentHp - 20;
                Debug.Log(currentHp);
                Invoke(nameof(HitReset), 3);
            }
        }
        void HitReset()
        {
            playerGotHit = false;
        }
}
