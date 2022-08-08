using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraShake
{
    Hit,
    Explosion
}

public class CameraController : MonoBehaviour
{
    public GameObject target;       // Object we want to follow.
    public Vector3 offset;          // Global position offset from the target.
    public float accelerateFov;     // FOV to lerp to when we accelerate.

    private Camera cam;

    private float defaultFov;       // FOV we start the game at.
    private float targetFov;        // Target FOV we want to lerp to.
    private bool shaking;           // Are we currently shaking the camera?

    // Instance
    public static CameraController inst;

    void Awake ()
    {
        inst = this;
        cam = GetComponentInChildren<Camera>();

        // Set the FOV values.
        defaultFov = cam.fieldOfView;
        targetFov = cam.fieldOfView;
    }

    void Update ()
    {
        // If we have a target, follow them.
        if(target)
            transform.position = target.transform.position + offset;

        // Lerp towards the target FOV.
        if(cam.fieldOfView != targetFov)
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, 2 * Time.deltaTime);
    }

    // Called when the player starts accelerating.
    public void OnAccelerationStart ()
    {
        targetFov = accelerateFov;
    }

    // Called when the player stops accelerating.
    public void OnAccelerationEnd ()
    {
        targetFov = defaultFov;
    }

    // Shake the camera a certain way.
    public void Shake (CameraShake shakeType)
    {
        switch(shakeType)
        {
            case CameraShake.Hit:
                StartCoroutine(Shake(0.2f, 0.5f, 30.0f));
                break;
            case CameraShake.Explosion:
                StartCoroutine(Shake(0.4f, 2.0f, 20.0f));
                break;
        }
    }

    // Shake the camera.
    IEnumerator Shake (float duration, float amount, float intensity)
    {
        // Return if we're already shaking.
        if(shaking)
            yield break;

        shaking = true;

        Vector3 targetPos = Vector3.zero;
        float time = duration;

        while(time > 0.0f)
        {
            // Move the camera towards the target pos.
            cam.transform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, targetPos, intensity * Time.deltaTime);

            // If the camera is at the target pos - get a new one.
            if(cam.transform.localPosition == targetPos)
                targetPos = Random.insideUnitSphere * amount;

            time -= Time.deltaTime;
            yield return null;
        }

        shaking = false;

        // Reset the camera position.
        cam.transform.localPosition = Vector3.zero;
    }
}