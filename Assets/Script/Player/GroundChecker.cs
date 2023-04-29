using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GroundChecker : MonoBehaviour
{
    public bool isGrounded => collisions.Length > 0;

    [SerializeField] private LayerMask groundLayer;

    public Collider2D[] collisions { get; private set; }

    private void FixedUpdate()
    {
        collisions = Physics2D.OverlapBoxAll(transform.position, new Vector2(2, 0.2f),0,groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector2(1, 0.2f));
    }

}
