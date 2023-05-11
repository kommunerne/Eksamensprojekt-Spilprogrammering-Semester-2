using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // variables
    public int maxhealth = 100; // The amount of health the unit has
    public int moveSpeed = 10;
    public int currentHealth;
    public bool isRegenHealth;
    private float healthRegenRate = 0.5f;
    private bool damageTaken;
    private Rigidbody rb;
    public int damage;
    public string playerTag = "red";
    public float fireRate;
    public float bulletSpeed = 10f;
    public float bulletlifetime = 3f;
    public bool readyToShoot;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // health regen coroutine
        if (damageTaken == false)
        {
            if (currentHealth != maxhealth && !isRegenHealth)
            {
                StartCoroutine("regeainHealthOverTime");
            }
        }

        //movement

        if (Input.GetKey(KeyCode.A))
            rb.AddForce(Vector3.left);
        if (Input.GetKey(KeyCode.D))
            rb.AddForce(Vector3.right);
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(Vector3.up);
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(Vector3.down);

        //shooting function
        if (Input.GetKey(KeyCode.Space) && readyToShoot == true)
        {
            readyToShoot = false;
        }
    }

    //Check for damage taken
    private IEnumerator haveTakenDamage()
    {
        damageTaken = true;
        yield return new WaitForSeconds(5f);
    }

    //health regen
    private IEnumerator regeainHealthOverTime()
    {
        isRegenHealth = true;
        while (currentHealth < maxhealth)
        {
            currentHealth++;
            yield return new WaitForSeconds(healthRegenRate);
        }
        isRegenHealth = false;
    }

    //damage taken / kill
    public void Damage(int damage)
    {
        if (damage < 0)
        {
            throw new System.ArgumentOutOfRangeException("Damage cannot be negative"); // Since we dont want to be capable of dealing negative damage, i've made the script throw an error
        }

        currentHealth -= damage; // This script is made to allow someone to damage this character
        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    //kill command
    private void Kill()
    {
        this.gameObject.SetActive(false);
    }

    //damage checker
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == playerTag || collision.gameObject.tag == "enemy")
        {
            Health health = collision.GetComponent<Health>();
            health.Damage(damage);

        }
    }

    //firerate
    private IEnumerator fireRateShooting()
    {
        readyToShoot = true;
        yield return new WaitForSeconds(fireRate);
    }
}
