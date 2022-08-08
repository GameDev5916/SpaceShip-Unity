using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShooting : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject projectilePrefab;     // Prefab of the projectile we're shooting.
    public GameObject laserPrefab;
    public float shootRate;                 // Min time between shots.
    private float lastShootTime;            // Last time we shot a projectile.
    public float projectileSpeed;           // Initial velocity of the projectile.

    public Transform mineShoot;

    [Header("Projectile Modifiers")]
    public float projectileSpeedModifier = 1.0f;    // Multiplied by the default projectile speed when projectile is created.

    [Header("Components")]
    public Transform[] muzzles;             // Array of all muzzles which the projectiles will come out from.
    private int curMuzzle;                  // Index of the current muzzle we're shooting from.
    private Rigidbody rig;                  // Our 'Rigidbody' component.
    private ParticleSystem[] muzzleFlashParticles;  // Array of all muzzle flash particles (set when the game is played).
    private ParticleSystem mineShotParticles;
    private AudioSource audioSource;

    void Awake()
    {
        // Get the components.
        rig = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // Set the muzzle flash particles (if we have any).
        if(muzzles[0].transform.childCount > 0)
        {
            muzzleFlashParticles = new ParticleSystem[muzzles.Length];

            for(int x = 0; x < muzzles.Length; ++x)
                muzzleFlashParticles[x] = muzzles[x].GetComponentInChildren<ParticleSystem>();
        }
    }

    // Can we shoot a projectile?
    public bool CanShoot ()
    {
        return Time.time - lastShootTime >= shootRate;
    }

    // Shoots a projectile forwards.
    public void Shoot ()
    {
        lastShootTime = Time.time;

        if (GameManager.inst.scoreMode == 0)
        {
            // Instantiate the projectile prefab.
            GameObject projectile = Pool.inst.Spawn(projectilePrefab, muzzles[curMuzzle].position, muzzles[curMuzzle].rotation);
            Projectile projScript = projectile.GetComponent<Projectile>();

            projScript.rig.velocity = rig.velocity + (muzzles[curMuzzle].forward * (projectileSpeed * projectileSpeedModifier)) + rig.velocity;
            projScript.shooterObj = gameObject;

            // Play a muzzle flash particle (if we have one).
            if (muzzleFlashParticles != null)
                muzzleFlashParticles[curMuzzle].Play();

            // Set the next muzzle to shoot a projectile.
            curMuzzle++;

            if (curMuzzle == muzzles.Length)
                curMuzzle = 0;
        }
        else // Shoot for mining
        {
            // Instantiate the projectile prefab.
            GameObject projectile = Pool.inst.Spawn(laserPrefab, mineShoot.position, mineShoot.rotation);
            Projectile projScript = projectile.GetComponent<Projectile>();

            //projScript.rig.velocity = rig.velocity + (mineShoot.forward * (projectileSpeed * projectileSpeedModifier)) + rig.velocity;
            projScript.shooterObj = gameObject;
            projectile.transform.localScale = new Vector3(0.3f, 0.3f, 1f);

            // Play a muzzle flash particle (if we have one).
            if (mineShotParticles != null)
                mineShotParticles.Play();

        }
        // Play sound effect.
        AudioManager.inst.Play(AudioManager.inst.shipShoot, audioSource, true);
    }
}