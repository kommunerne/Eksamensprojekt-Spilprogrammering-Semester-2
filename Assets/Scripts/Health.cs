using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxhealth = 100; // The amount of health the unit has
    public int moveSpeed = 10;
    public int fireRate = 10;
    private int currentHealth;
    private bool isRegenHealth;
    private float healthRegenRate = 0.5f;
    private bool damageTaken;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (damageTaken == false) {
            if (currentHealth != maxhealth && !isRegenHealth)
            {
                StartCoroutine("regeainHealthOverTime");
            }
        }
    }

    private IEnumerator haveTakenDamage()
    {
        damageTaken = true;
        yield return new WaitForSeconds(5f);
    }
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

    public void update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (Input.GetKey(KeyCode.A))
            rb.AddForce(Vector3.left);
        if (Input.GetKey(KeyCode.D))
            rb.AddForce(Vector3.right);
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(Vector3.up);
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(Vector3.down);
    } 

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

    private void Kill()
    {
        this.gameObject.SetActive(false);
    }
}
