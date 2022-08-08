using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : Item
{
    public int health;
    public Material shieldMaterial;
    private Collider coll;

    // Called when the object is spawned in.
    public override void Initialize ()
    {
        // Make the shield a child of the player.
        transform.parent = PlayerController.inst.transform;
        transform.localPosition = Vector3.zero;

        coll = GetComponent<Collider>();
    }

    void Update ()
    {
        // Make the shield material move over time.
        shieldMaterial.mainTextureOffset = new Vector3(0, Time.time);
    }

    void OnTriggerEnter (Collider other)
    {
        // Did we get hit by a projectile?
        if(other.CompareTag("Projectile"))
        {
            // Let the player's bullets pass.
            if(other.GetComponent<Projectile>().shooterObj == PlayerController.inst.gameObject)
                return;

            // Reduce health.
            health--;

            if(health <= 0)
                DestroyItem();

            // Destroy the bullet.
            Pool.inst.Destroy(other.gameObject);
        }
    }
}