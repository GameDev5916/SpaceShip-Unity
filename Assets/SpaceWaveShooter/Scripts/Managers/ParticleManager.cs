using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all the particles we can spawn in.
/// </summary>
public class ParticleManager : MonoBehaviour
{
    [Header("Ship")]
    public ParticleSystem explosion;
    public ParticleSystem hit;

    // Instance
    public static ParticleManager inst;

    void Awake ()
    {
        // Set the instance to this script.
        inst = this;
    }

    // Creates a particle.
    public void Spawn (ParticleSystem particlePrefab, Vector3 position)
    {
        // Spawn the particle from the pool.
        ParticleSystem particle = Pool.inst.Spawn(particlePrefab.gameObject, position, Quaternion.identity).GetComponent<ParticleSystem>();

        // Play the particle.
        particle.Play();

        // Destroy the particle after it's played.
        Pool.inst.Destroy(particle.gameObject, particlePrefab.main.duration);
    }
}