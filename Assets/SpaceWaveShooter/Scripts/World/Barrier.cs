using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public float barrierRadius;
    public Rigidbody objectToClamp;
    public SpriteRenderer visual;
    public float visualAppearRange;

    void Update ()
    {
        // Clamp the object's position inside of the barrier circle.
        Vector3 pos = objectToClamp.transform.position - transform.position;
        pos = Vector3.ClampMagnitude(pos, barrierRadius);
        objectToClamp.MovePosition(transform.position + pos);

        // Position the visual.
        Vector3 dir = (objectToClamp.transform.position - transform.position).normalized;
        visual.transform.position = dir * barrierRadius;
        visual.transform.forward = dir;

        ManageVisual();
    }

    void ManageVisual ()
    {
        float dist = Vector3.Distance(transform.position, visual.transform.position);

        float rate = (barrierRadius - visualAppearRange) - dist;
        Debug.Log(rate);
        Color col = visual.color;
        col.a = rate;
        visual.color = col;
    }

    void OnDrawGizmos ()
    {
        #if UNITY_EDITOR

        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, barrierRadius);

        #endif
    }
}