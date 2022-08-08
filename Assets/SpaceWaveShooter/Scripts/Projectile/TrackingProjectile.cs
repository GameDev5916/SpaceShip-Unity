using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Chases after a target and can be shot down.
/// </summary>
public class TrackingProjectile : Projectile, IDamagable
{
    private GameObject target;
    public float trackingSpeed;         // Speed the projectile chases its target at.
    public float lookAtRate;            // Rate applied to the lerp to look at the target.
    public int hitThreshold;            // How many times can the projectile be hit before getting destroyed?

    void Start ()
    {
        // Set the player as the target.
        target = PlayerController.inst.gameObject;
    }

    void Update ()
    {
        LookAtTarget();
    }

    void FixedUpdate ()
    {
        // Set our velocity.
        rig.velocity = transform.forward * trackingSpeed;
    }

    // Rotates to look at the target position.
    public void LookAtTarget ()
    {
        // Get the direction between us and the target position.
        Vector3 dir = (target.transform.position - transform.position).normalized;

        // Calculate a Y angle.
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z), lookAtRate * Time.deltaTime);
    }

    // Called when the projectile gets shot.
    public void TakeDamage (int damage)
    {
        hitThreshold--;

        if(hitThreshold == 0)
            Pool.inst.Destroy(gameObject);
    }
}