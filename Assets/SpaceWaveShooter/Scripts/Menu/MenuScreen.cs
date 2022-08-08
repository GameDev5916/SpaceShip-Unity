using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all menu screens.
/// </summary>
public class MenuScreen : MonoBehaviour
{
    void OnEnable ()
    {
        OnOpenScreen();
    }

    // Called when we open the screen.
    public virtual void OnOpenScreen ()
    {

    }
}