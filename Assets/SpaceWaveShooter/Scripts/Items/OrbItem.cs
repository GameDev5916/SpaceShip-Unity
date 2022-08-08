using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbItem : Item
{
    private GameObject player;

    public float rotateSpeed;
    public float distanceFromPlayer;
    public int damage;

    private float curAngle;

    // Called when the object is spawned in.
    public override void Initialize ()
    {
        player = PlayerController.inst.gameObject;
        transform.position = player.transform.position + (player.transform.forward * distanceFromPlayer);
    }

    void Update ()
    {
        // Rotate around the player.
        Vector3 dir = Quaternion.AngleAxis(curAngle, Vector3.up) * Vector3.right;
        curAngle += rotateSpeed * Time.deltaTime;

        transform.position = player.transform.position + (dir * distanceFromPlayer);

        if(curAngle >= 360.0f)
            curAngle = curAngle - 360.0f;
    }

    void OnTriggerEnter(Collider other)
    {
        // See if the object we hit is damagable.
        IDamagable damagable = other.GetComponent<IDamagable>();

        // If it is, damage it.
        if(damagable != null && other.gameObject != player)
        {
            damagable.TakeDamage(damage);
        }
    }
}