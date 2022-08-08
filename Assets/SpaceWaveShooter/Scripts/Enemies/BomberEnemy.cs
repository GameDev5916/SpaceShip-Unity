using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : Ship
{
    [Header("Stats")]
    public float lookAtRate;
    private GameObject target;
    public int damage;
    public int moneyToGiveOnDeath;

    [Header("Components")]
    public MeshRenderer bomberHead;

    public override void Awake ()
    {
        // get the target
        target = PlayerController.inst.gameObject;

        base.Awake();
    }

    void OnEnable ()
    {
        curHp = maxHp;

        if(modelMeshRenderers == null)
            return;

        // Change the model color to the default.
        for(int x = 0; x < modelMeshRenderers.Length; ++x)
            modelMeshRenderers[x].material.color = modelDefaultColors[x];

        StopCoroutine(FlashBomberHead());
        StartCoroutine(FlashBomberHead());
    }

    IEnumerator FlashBomberHead ()
    {
        while(true)
        {
            if(bomberHead.material.color == Color.white)
                bomberHead.material.color = Color.red;
            else
                bomberHead.material.color = Color.white;

            yield return new WaitForSeconds(0.1f);
        }
    }

    void Update ()
    {
        // Look at the target.
        if(target)
            LookAt(target.transform.position, lookAtRate);
    }

    public override void FixedUpdate ()
    {
        // Accelerate towards the target.
        if(target)
            Accelerate();
    }

    void OnTriggerEnter (Collider other)
    {
        // Did we hit the player?
        if(other.CompareTag("Player"))
        {
            Explode();
        }
    }

    void Explode ()
    {
        // Damage player.
        PlayerController.inst.TakeDamage(damage);

        // Explosion particle effect.
        ParticleManager.inst.Spawn(ParticleManager.inst.explosion, transform.position);

        // Camera shake.
        CameraController.inst.Shake(CameraShake.Explosion);

        // Tell the enemy manager we've died.
        if(EnemyManager.inst.onEnemyDeath != null)
            EnemyManager.inst.onEnemyDeath.Invoke();

        // Play sound effect.
        AudioManager.inst.PlayWithTempAudioSource(AudioManager.inst.shipExplode, transform.position);

        Pool.inst.Destroy(gameObject);
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