using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float fireRate;
    public float bulletSpeed = 10f;
    public float bulletlifetime = 3f;
    public bool readyToShoot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private IEnumerator fireRateShooting()
    {
        readyToShoot = true;
        yield return new WaitForSeconds(fireRate);
    }
}
