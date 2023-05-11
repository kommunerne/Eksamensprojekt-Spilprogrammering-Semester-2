using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float fireRate;
    public float bulletSpeed = 10f;
    public float bulletlifetime = 3f;
    private Vector3 mousePos;
    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToViewportPoint(Input.mousePosition);
        
        Vector3 rotation = mousePos - transform.position;

        float rot2 = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rot2);
    }
}
