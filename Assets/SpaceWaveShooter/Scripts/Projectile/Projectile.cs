using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all projectiles.
/// </summary>
public class Projectile : MonoBehaviour
{
    private float spawnTime;                    // Time the projectile was spawned.

    [HideInInspector]
    public GameObject shooterObj;               // Object that shot the projectile (so we can't hit ourselves).

    [Header("Projectile")]
    public int damage;
    public float defaultDestroyTime = 3.0f;     // Life time of the projectile.
    public Rigidbody rig;

    // OnEnable instead of Start since we're pooling the projectiles.
    void OnEnable ()
    {
        spawnTime = Time.time;
    }
    
    void Update ()
    {
        // Destroy the projectile after a set amount of time.
        if (GameManager.inst.scoreMode == 1)
        {
            transform.localScale = new Vector3(0.3f, 0.3f, transform.localScale.z + 2f);
            transform.Translate(0, 0, 2f);
        }
        else
        {
            transform.localScale = new Vector3(0.3f ,0.3f,1);
        }
        if (Time.time - spawnTime > defaultDestroyTime)
            Pool.inst.Destroy(gameObject);
    }

    public virtual void OnTriggerEnter (Collider other)
    {
        // See if the object we hit is damagable.
        IDamagable damagable = other.GetComponent<IDamagable>();

        // If it is, damage it.
        if(damagable != null && other.gameObject != shooterObj)
        {
            // Did we hit a bullet? And if so did it come from the same shooter?
            Projectile proj = other.GetComponent<Projectile>();

            if(proj != null && proj.shooterObj == shooterObj)
                return;

            // Damage the damagable.
            damagable.TakeDamage(damage);

            // Create a hit particle.
            ParticleManager.inst.Spawn(ParticleManager.inst.hit, transform.position);

            // Destroy the projectile.
            Pool.inst.Destroy(gameObject);
        }
    }
}