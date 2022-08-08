using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for all ships.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour, IDamagable
{
    [Header("Stats")]
    public int curHp;                           // Current health.
    public int maxHp;                           // Maximum health.

    [Header("Movement")]
    public float accelerationSpeed;             // Force applied forwards to accelerate.
    public GameObject engineVisual;             // Visual to show that the ship is accelerating.

    private bool accelerating;                  // Are we currently accelerating?
    private bool damageFlashing;                // Are we currently damage flashing?

    // Events
    [HideInInspector]
    public System.Action onAccelerationStart;   // Called when we start to accelerate.
    [HideInInspector]
    public System.Action onAccelerationEnd;     // Called when we stop accelerating.

    [HideInInspector] public Rigidbody rig;
    private AudioSource audioSource;

    [HideInInspector] public MeshRenderer[] modelMeshRenderers;     // All the mesh renderers that make up the model.
    [HideInInspector] public Color[] modelDefaultColors;            // Default color for each mesh renderer.

    public virtual void Awake ()
    {
        // Get the components.
        rig = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // Get all mesh renderers in the model.
        modelMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        modelDefaultColors = new Color[modelMeshRenderers.Length];

        // Get the mesh renderer's default colors.
        for(int x = 0; x < modelDefaultColors.Length; ++x)
            modelDefaultColors[x] = modelMeshRenderers[x].material.color;
    }

    // Adds force forwards.
    public void Accelerate ()
    {
        rig.AddForce(transform.forward * accelerationSpeed);

        // Enable the engine visual.
        if(!engineVisual.activeInHierarchy)
            engineVisual.SetActive(true);

        // Call the onAccelerationStart event.
        if(!accelerating && onAccelerationStart != null)
            onAccelerationStart.Invoke();

        accelerating = true;
    }

    public virtual void FixedUpdate ()
    {
        if(engineVisual == null)
            return;

        // If we're not accelerating and the engine visual is on - disable it.
        if(!accelerating)
        {
            engineVisual.SetActive(false);

            // Call the onAccelerationEnd event.
            if(onAccelerationEnd != null)
                onAccelerationEnd.Invoke();
        }

        accelerating = false;
    }

    // Rotates to look at the target position.
    public void LookAt (Vector3 targetPos, float lerpRate = 0.0f)
    {
        // Get the direction between us and the target position.
        Vector3 dir = (targetPos - transform.position).normalized;

        // Calculate a Y angle.
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        // If we have a lerp rate - apply it to a lerp.
        if(lerpRate != 0.0f)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z), lerpRate * Time.deltaTime);
        // Otherwise, don't lerp the rotation.
        else transform.rotation = Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z);
    }

    // Called when the ship gets damaged.
    public virtual void TakeDamage (int damage)
    {
        // Take away the damage.
        curHp -= damage;

        // Flash the model color to white.
        if(!damageFlashing)
            StartCoroutine(DamageFlash());

        // Play sound effect.
        AudioManager.inst.Play(AudioManager.inst.shipHit, audioSource, true);

        // Did we die?
        if(curHp <= 0)
        {
            curHp = 0;
            Die();
        }
    }

    // Called when the ship's health reaches 0.
    public virtual void Die ()
    {
        
    }

    // Flashed the model color to white when we get damaged.
    IEnumerator DamageFlash ()
    {
        damageFlashing = true;

        // Change the model color to white.
        for(int x = 0; x < modelMeshRenderers.Length; ++x)
            modelMeshRenderers[x].material.color = Color.white;

        // Wait a very short amount of time.
        yield return new WaitForSeconds(0.05f);

        // Change the model color to the default.
        for(int x = 0; x < modelMeshRenderers.Length; ++x)
            modelMeshRenderers[x].material.color = modelDefaultColors[x];

        damageFlashing = false;
    }
}