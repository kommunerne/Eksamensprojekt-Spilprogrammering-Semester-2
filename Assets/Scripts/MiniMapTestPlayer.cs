using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MiniMapTestPlayer : MonoBehaviour
{
    
    public Camera cameraToUse;
    public GameObject test;
    private Rigidbody2D rb;
    [SerializeField] private PlayerUIController uiController;
        
    
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
    public ulong sceenCullingMask;
    public string playerName;
    public int pinCode;
    public bool playerGotHit;
    private bool _playerDead = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        uiController = GetComponent<PlayerUIController>();
        playerGotHit = false;
    }

    void Update()
    {
       // Hit();
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (!_playerDead)
        {
            Vector2 movement = new Vector2(horizontalInput, verticalInput); 
            rb.velocity = movement.normalized * moveSpeed;
        }
            


        if(!_playerDead)
            RotateToMouse2D();
    }

    void RotateToMouse2D(){

        // Get the position of the mouse in world space
        Vector3 mousePosition = cameraToUse.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the angle between the mouse and the player's position
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the player to face the mouse
        test.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void HpUpgrade()
    {
        maxHp += 100;
        currentHp += 100;
        statPoints--;
    }

    public void DmgUpgrade()
    {
        dmg += 20;
        statPoints--;
    }

    public void FirerateUpgrade()
    {
        firerate += 0.15f;
        statPoints--;
    }

    public void MovespeedUpgrade()
    {
        moveSpeed += 1f;
        statPoints--;
    }

    public void HpregenUpgrade()
    {
        hpregen += 5;
        statPoints--;
    }

    /*public void Hit()
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
                cameraToUse.cullingMask = 119;
                rb.velocity = new Vector2(0,0);
            }
            currentHp = currentHp - 20;
            Debug.Log(currentHp);
            Invoke(nameof(HitReset), 3);
        }
    }*/

    void HitReset()
    {
        playerGotHit = false;
    }
}
