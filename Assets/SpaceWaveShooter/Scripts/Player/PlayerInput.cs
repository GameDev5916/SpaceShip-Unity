using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum InputType
{
    KeyboardMouse,
    Controller,
    Mobile
}
public enum InputState
{
    Down,
    Held,
    Up
}

public class PlayerInput : MonoBehaviour
{
    public InputType inputType;             // Type of input method used to control the player.
    public GameObject mobileUIControls;     // Mobile controls to enable / disable.

    [Header("Keyboard & Mouse")]
    public KeyCode kmAccelerateKey = KeyCode.Space;                     // Key to accelerate on keyboard and mouse.
    public KeyCode kmShootKey = KeyCode.Mouse0;                         // Key to shoot on keyboard and mouse.

    [Header("Controller")]
    public KeyCode controllerAccelerateKey = KeyCode.JoystickButton5;   // Key to accelerate on controller.
    public KeyCode controllerShootKey = KeyCode.JoystickButton0;        // Key to shoot on controller.

    private KeyCode curAccelerateKey;           // Current key to accelerate (assigned on start of game).
    private KeyCode curShootKey;                // Current key to shoot (assigned on start of game).

    [Header("Mobile")]
    public MobileJoystick mobileRotateJoystick;

    // Components
    private PlayerController playerController;
    private Camera cam;

    void Start ()
    {
        // Get the components.
        playerController = GetComponent<PlayerController>();
        cam = Camera.main;

        // Enable the mobile controls if we're on mobile.
        if(mobileUIControls != null)
            mobileUIControls.SetActive(inputType == InputType.Mobile);

        // Assign current keys for keyboard/mouse or controller.
        if(inputType != InputType.Mobile)
        {
            curAccelerateKey = inputType == InputType.KeyboardMouse ? kmAccelerateKey : controllerAccelerateKey;
            curShootKey = inputType == InputType.KeyboardMouse ? kmShootKey : controllerShootKey;
        }
    }

    void Update ()
    {
        // If we're on mobile, there's no need to check this stuff.
        if(inputType == InputType.Mobile)
        {
            if(mobileRotateJoystick.dir.magnitude != 0.0f)
                playerController.UpdateLookDirection(transform.position + mobileRotateJoystick.dir);

            return;
        }

        // Checking for 'Accelerate' key.
        // Down, hold and up key states.
        if(Input.GetKeyDown(curAccelerateKey))
            playerController.OnAccelerateButton(InputState.Down);
        else if(Input.GetKey(curAccelerateKey))
            playerController.OnAccelerateButton(InputState.Held);
        else if(Input.GetKeyUp(curAccelerateKey))
            playerController.OnAccelerateButton(InputState.Up);

        // Checking for 'Shoot' key.
        // Down, hold and up key states.
        if(Input.GetKeyDown(curShootKey) && !EventSystem.current.IsPointerOverGameObject())
            playerController.OnShootButton(InputState.Down);
        else if(Input.GetKey(curShootKey) && !EventSystem.current.IsPointerOverGameObject())
            playerController.OnShootButton(InputState.Held);
        else if(Input.GetKeyUp(curShootKey))
            playerController.OnShootButton(InputState.Up);

        // Rotate the player to face the mouse position.
        if(inputType == InputType.KeyboardMouse)
            playerController.UpdateLookDirection(GetWorldMousePos());
        // Rotate the player to face the controller joystick direction.
        else if(inputType == InputType.Controller)
        {
            Vector3 dir = GetControllerJoystickDirection();

            // Only update the rotation if we're moving the joystick.
            if(dir.magnitude != 0.0f)
                playerController.UpdateLookDirection(playerController.transform.position + dir);
        }
    }

    // Returns the direction of the controller's left joystick axis.
    // Used for rotating the player on 'Controller'.
    Vector3 GetControllerJoystickDirection ()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        return new Vector3(x, 0, z);
    }

    // Returns the world position of the mouse on the same axis as the player.
    // Used for rotating the player on 'Keyboard and Mouse'.
    Vector3 GetWorldMousePos ()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;

        if(plane.Raycast(ray, out enter))
            return ray.GetPoint(enter);

        return Vector3.zero;
    }

    // Called when the mobile "Accelerate" button is pressed down.
    public void OnMobileAccelerateButton (InputState state)
    {
        playerController.OnAccelerateButton(state);
    }

    // Called when the mobile "Shoot" button is pressed down.
    public void OnMobileShootButton (InputState state)
    {
        playerController.OnShootButton(state);
    }
}