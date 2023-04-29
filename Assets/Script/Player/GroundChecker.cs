using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GroundChecker : MonoBehaviour
{
    public bool isGrounded;
    [SerializeField] private Collider2D checkerCollider;
    [SerializeField] private LayerMask groundLayer;

    private int layerToCheck => Utility.LayerMaskToLayer(groundLayer);

    public Collider2D[] collisions { get; private set; }

    private void FixedUpdate()
    {
        isGrounded = checkerCollider.IsTouchingLayers(groundLayer);
    }

}
