using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapTestPlayer : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Camera cameraToUse;
    public GameObject test;
    private Rigidbody2D rb;
    
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
}
