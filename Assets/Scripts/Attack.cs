using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public string playerTag = "red";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == playerTag || collision.gameObject.tag == "enemy")
        {
            Health health = collision.GetComponent<Health>();
            health.Damage(damage);

        }
    }

}
