using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapTestPlayer : MonoBehaviour
{
    
    public Camera cameraToUse;
    public GameObject test;
    private Rigidbody2D rb;

    public int hp = 100;
    public int dmg = 20;
    public float firerate = 1.0f;
    public float moveSpeed = 5f;
    public int hpregen = 5;

    public int level;
    public int exp;
    public int statPoints;
    public int score;

    public string name;
    public int pinCode;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        rb.velocity = movement.normalized * moveSpeed;

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
        hp += 100;
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
}
