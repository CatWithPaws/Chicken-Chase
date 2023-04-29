using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Collider2D defaultCollider;
    [SerializeField] private Collider2D slideCollider;

    [SerializeField] private TouchHandler touchHandler;

    [SerializeField] private GroundChecker groundChecker;

    [SerializeField] private PlayerState playerState;

	private void Awake()
	{
        touchHandler.OnTouchMove += HandleTouch;
	}

	private void FixedUpdate()
	{
		if(playerState == PlayerState.Fall && groundChecker.isGrounded)
        {
            Slide();
        }
	}

	private void HandleTouch(Vector2 moveVector)
    {
        if (moveVector.y > 0)
        {
            Jump();
        }
        else if(moveVector.y < 0)
        {
            SlideOrFastFall();
        }
    }

	private void Jump()
    {
        if (groundChecker.isGrounded)
        {

        }
    }

    private void SlideOrFastFall()
    {
        if(groundChecker.isGrounded)
        {
            Slide();
        }
        else
        {
            FastFallThenSlide();
        }
    }

    private void Slide()
    {

    }

    private void FastFallThenSlide()
    {

    }

}

enum PlayerState
{
    Idle, Run, Jump, Fall, Slide
}