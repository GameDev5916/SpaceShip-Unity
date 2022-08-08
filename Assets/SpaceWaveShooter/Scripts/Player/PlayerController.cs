using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ShipShooting))]
public class PlayerController : Ship
{
    [Header("Components")]
    public ShipShooting shooting;
    private Camera cam;

    private bool accelerateKeyDown;

    // Instance
    public static PlayerController inst;

    public override void Awake ()
    {
        inst = this;
        base.Awake();
    }

    #region Susbcribe to Events

    void OnEnable ()
    {
        onAccelerationStart += CameraController.inst.OnAccelerationStart;
        onAccelerationEnd += CameraController.inst.OnAccelerationEnd;
    }

    void OnDisable ()
    {
        onAccelerationStart -= CameraController.inst.OnAccelerationStart;
        onAccelerationEnd -= CameraController.inst.OnAccelerationEnd;
    }

    #endregion

    void Start ()
    {
        cam = Camera.main;

        // Initialize health bar UI.
        GameUI.inst.InitializeHealthBar(maxHp);
    }

    void Update ()
    {
        // Try to shoot when we press the 'Left Mouse Button'.
        if(Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            if(shooting.CanShoot())
                shooting.Shoot();
    }

    // Called when the accelerate button is pressed.
    // Called from the 'PlayerInput' script.
    public void OnAccelerateButton (InputState state)
    {
        if(state == InputState.Held || state == InputState.Down)
            accelerateKeyDown = true;
        else
            accelerateKeyDown = false;
    }

    // Called when the shoot button is pressed.
    // Called from the 'PlayerInput' script.
    public void OnShootButton (InputState state)
    {
        if(shooting.CanShoot())
            shooting.Shoot();
    }

    // Updates the player input corresponding to the ship's look direction.
    // Called form the 'PlayerInput' script.
    public void UpdateLookDirection (Vector3 dir)
    {
        LookAt(dir);
    }

    public override void FixedUpdate ()
    {
        // Accelerate when we hold down the 'SPACE' button.
        if(accelerateKeyDown)
            Accelerate();

        base.FixedUpdate();
    }

    // Called when we get hit by a projectile.
    public override void TakeDamage (int damage)
    {
        base.TakeDamage(damage);

        // Shake the camera.
        CameraController.inst.Shake(CameraShake.Hit);

        // Update health bar UI.
        GameUI.inst.UpdateHealthBar(curHp);
    }

    // Called when the player purchases a stat upgrade.
    public void UpgradeStat (UpgradeType statType, float modifierToApply)
    {
        switch(statType)
        {
            case UpgradeType.Speed:
                accelerationSpeed *= modifierToApply;
                break;
            case UpgradeType.FireRate:
                shooting.shootRate /= modifierToApply;
                break;
        }
    }

    // Give the player an amount of health.
    public void Heal (int amount)
    {
        curHp = Mathf.Clamp(curHp + amount, 0, maxHp);

        // Update health bar UI.
        GameUI.inst.UpdateHealthBar(curHp);
    }

    public override void Die ()
    {
        GameManager.inst.PlayerDied();
    }
}