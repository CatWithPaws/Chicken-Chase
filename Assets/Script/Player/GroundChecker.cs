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

    [SerializeField] private PlayerController player;


    public Collider2D[] collisions { get; private set; }

    private void FixedUpdate()
    {
        isGrounded = checkerCollider.IsTouchingLayers(groundLayer);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.TryGetComponent(out EnemyBlock enemy))
        {
            player.OnEnemyTouch(enemy);
		}
	}
}
