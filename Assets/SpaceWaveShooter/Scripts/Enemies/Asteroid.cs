using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Ship
{
    [Header("Stats")]
    public float lookAtRate;            // Applied to the look at rotation lerp.
    private GameObject target;
    public int score;
    public int scale;

    void Start()
    {
        // Get the default target - the player.
        target = PlayerController.inst.gameObject;
    }

    void OnEnable()
    {
        curHp = maxHp;

        if (modelMeshRenderers == null)
            return;

        // Change the model color to the default.
        for (int x = 0; x < modelMeshRenderers.Length; ++x)
            modelMeshRenderers[x].material.color = modelDefaultColors[x];
    }

    void Update()
    {   
    }

    public override void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 10)
            Accelerate();
        // Otherwise, shoot at the target.
        if(curHp != maxHp)
        {
            this.gameObject.transform.localScale = new Vector3((float)curHp / maxHp, (float)curHp / maxHp, (float)curHp / maxHp);
            if((float)curHp/maxHp < 0.3)
            {
                Die();
            }
        }
        base.FixedUpdate();
    }

    // Called when our health reaches 0.
    public override void Die()
    {
        // Explosion particle effect when Asteroid dies 
//      ParticleManager.inst.Spawn(ParticleManager.inst.explosion, transform.position);

        // Camera shake.
        CameraController.inst.Shake(CameraShake.Explosion);

        // Give the player score.
        GameManager.inst.GiveScore(score);

        // Tell the enemy manager we've died.
        if (EnemyManager.inst.onEnemyDeath != null)
            EnemyManager.inst.onEnemyDeath.Invoke();

        // Play sound effect.
        AudioManager.inst.PlayWithTempAudioSource(AudioManager.inst.shipExplode, transform.position);

        Pool.inst.Destroy(gameObject);
    }
}
