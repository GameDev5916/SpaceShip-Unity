using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipShooting))]
public class SpitterEnemy : Ship
{
    [Header("Stats")]
    public float rotateSpeed;
    private GameObject target;
    public int moneyToGiveOnDeath;

    [Header("Components")]
    public ShipShooting shooting;

    public override void Awake ()
    {
        // get the target
        target = PlayerController.inst.gameObject;

        base.Awake();
    }

    void Update ()
    {
        Accelerate();
        
        // Rotate around Y axis.
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

        // Shoot when we can.
        if(shooting.CanShoot())
            shooting.Shoot();
    }

    // Called when our health reaches 0.
    public override void Die ()
    {
        // Explosion particle effect.
        ParticleManager.inst.Spawn(ParticleManager.inst.explosion, transform.position);

        // Camera shake.
        CameraController.inst.Shake(CameraShake.Explosion);

        // Give the player money.
        GameManager.inst.GiveMoney(moneyToGiveOnDeath);

        // Tell the enemy manager we've died.
        if(EnemyManager.inst.onEnemyDeath != null)
            EnemyManager.inst.onEnemyDeath.Invoke();

        // Play sound effect.
        AudioManager.inst.PlayWithTempAudioSource(AudioManager.inst.shipExplode, transform.position);

        Pool.inst.Destroy(gameObject);
    }
}