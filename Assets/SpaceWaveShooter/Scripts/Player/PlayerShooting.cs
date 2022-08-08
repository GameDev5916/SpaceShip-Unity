using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject projectilePrefab;     // Prefab of the projectile we're shooting.
    public float shootRate;                 // Min time between shots.
    private float lastShootTime;            // Last time we shot a projectile.
    public float projectileSpeed;           // Initial velocity of the projectile.

    [Header("Components")]
    public Transform[] muzzles;             // Array of all muzzles which the projectiles will come out from.
    private int curMuzzle;                  // Index of the current muzzle we're shooting from.
    private Rigidbody rig;

    void Awake ()
    {
        // Get the Rigidbody component.
        rig = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        if(Input.GetMouseButton(0))
        {
            if(Time.time - lastShootTime >= shootRate)
                Shoot();
        }
    }

    // Shoots a projectile forwards.
    void Shoot ()
    {
        lastShootTime = Time.time;

        // Instantiate the projectile prefab.
        GameObject projectile = Pool.inst.Spawn(projectilePrefab, muzzles[curMuzzle].position, transform.rotation);

        projectile.GetComponent<Rigidbody>().velocity = rig.velocity + (transform.forward * projectileSpeed);

        // Set the next muzzle to shoot a projectile.
        curMuzzle++;

        if(curMuzzle == muzzles.Length)
            curMuzzle = 0;
    }
}