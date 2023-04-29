using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PhysicsComponent : MonoBehaviour
{
    public Rigidbody2D rb;
	public float VerticalVelocity => rb.velocity.y;
	[SerializeField] private float jumpForce = 0.2f;

    public void Jump()
    {
		Vector2 newVelocity = rb.velocity;
		newVelocity.y = jumpForce * rb.mass;
		rb.velocity = newVelocity;
	}

	public void FastFall()
	{
		Vector2 newVelocity = rb.velocity;
		newVelocity.y = -jumpForce * rb.mass;
		rb.velocity = newVelocity;
	}
}
