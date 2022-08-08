using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool heldDown;          // Is the button being held down?

    public bool isShootButton;
    public bool isAccelerateButton;

    private PlayerInput input;

    void Awake ()
    {
        input = FindObjectOfType<PlayerInput>();
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        heldDown = true;
        transform.localScale = new Vector3(0.9f, 0.9f, 1.0f);

        if(isShootButton)
            input.OnMobileShootButton(InputState.Down);
        else if(isAccelerateButton)
            input.OnMobileAccelerateButton(InputState.Down);
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        heldDown = false;
        transform.localScale = Vector3.one;

        if(isShootButton)
            input.OnMobileShootButton(InputState.Up);
        else if(isAccelerateButton)
            input.OnMobileAccelerateButton(InputState.Up);
    }

    void Update ()
    {
        if(heldDown)
        {
            if(isShootButton)
                input.OnMobileShootButton(InputState.Held);
            else if(isAccelerateButton)
                input.OnMobileAccelerateButton(InputState.Held);
        }
    }
}
