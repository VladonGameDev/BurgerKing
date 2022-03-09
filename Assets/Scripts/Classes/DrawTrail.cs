using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrail : MonoBehaviour
{
    public bool activeTrail = true;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        trailRenderer = gameObject.GetComponent<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        if(activeTrail)
        {
            trailRenderer.emitting = true;
        }
        else
        {
            trailRenderer.emitting = false;
        }
    }
}
