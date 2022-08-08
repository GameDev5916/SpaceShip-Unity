using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipShooting))]
public class Enemy : Ship
{
    [Header("Stats")]
    public float lookAtRate;            // Applied to the look at rotation lerp.
    private GameObject target;
    public int moneyToGiveOnDeath;      // How much money does the enemy give to the player when they die?

    [Header("Ranges")]
    public float attackRange;           // Range the enemy will stop moving and start attacking.

    [Header("Components")]
    public ShipShooting shooting;

    void Start ()
    {
        // Get the default target - the player.
        target = PlayerController.inst.gameObject;

        if(GameManager.inst.curWave <= 10)
        {
            lookAtRate = 3 + GameManager.inst.curWave;
            maxHp += GameManager.inst.curWave;
        }
        else
            lookAtRate = 10 + GameManager.inst.curWave;
    }

    void OnEnable ()
    {
        curHp = maxHp;

        if(modelMeshRenderers == null)
            return;

        // Change the model color to the default.
        for(int x = 0; x < modelMeshRenderers.Length; ++x)
            modelMeshRenderers[x].material.color = modelDefaultColors[x];
    }

    void Update ()
    {
        // Do we have a target?
        if(target)
            LookAt(target.transform.position, lookAtRate);
    }

    public override void FixedUpdate ()
    {
        // If the target is outside of our attack range, accelerate towards them.
        if(Vector3.Distance(transform.position, target.transform.position) > attackRange)
            Accelerate();
        // Otherwise, shoot at the target.
        else if(shooting.CanShoot())
            shooting.Shoot();

        base.FixedUpdate();
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