using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isAlive;
  

    public bool IsAlive
    {
        get 
        { 
            return isAlive; 
        }
        private set
        {
            isAlive = value;
        }
    }
    [SerializeField] private Collider2D defaultCollider;
    [SerializeField] private Collider2D slideCollider;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private TouchHandler touchHandler;

    [SerializeField] private GroundChecker groundChecker;

    [SerializeField] private PlayerState playerState;
    private PlayerState PlayerState
    {
        get 
        { 
            return playerState; 
        }
        set
        {
            playerState = value;
            animations.SwitchAnimationTo(playerState);
        }
    }

    [SerializeField] private PhysicsComponent physics;
    [SerializeField] private AnimationComponent animations;

    private float slidingDuration = 0.5f;

    private float slideTimeLeft = 0f;
	private void Awake()
	{
        touchHandler.OnTouchMove += HandleTouch;
	}


	private void FixedUpdate()
	{
        CheckForSlideAfterFalling();
        CheckForSlideCoolDown();
        CheckForRunState();
	}

    public void Die()
    {
        IsAlive = false;
    }

    private void CheckForSlideCoolDown()
    {
        if (PlayerState != PlayerState.Slide)
        {
            return;
        }
        if(slideTimeLeft <= 0)
        {
			PlayerState = PlayerState.Run;
		}
        else
        {
            slideTimeLeft -= Time.fixedDeltaTime;
        }
    }


	private void CheckForSlideAfterFalling()
    {
		if (PlayerState == PlayerState.FastFall && groundChecker.isGrounded)
		{
            print("Slide after fall");
            Slide();
		}
	}

	private void CheckForRunState()
    {
        if(PlayerState == PlayerState.Jump)
        {
            if (groundChecker.isGrounded && physics.VerticalVelocity < 0)
            {
                PlayerState = PlayerState.Run;
            }
        }
    }

	private void HandleTouch(Vector2 moveVector)
    {
        if (moveVector.y > 0)
        {
            ResetSlideTimer();

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
            ResetSlideTimer();
			PlayerState = PlayerState.Jump;
            physics.Jump();
        }
    }


    private void SlideOrFastFall()
    {
        if(groundChecker.isGrounded)
        {
			PlayerState = PlayerState.Slide;
            Slide();
        }
        else
        {
            FastFall();
        }
    }

    private void Slide()
    {
        PlayerState = PlayerState.Slide;
        SwitchCollider();
        slideTimeLeft = slidingDuration;
    }

    private void SwitchCollider()
    {
		defaultCollider.enabled = playerState != PlayerState.Slide;
		slideCollider.enabled = playerState == PlayerState.Slide;
	}

    private void FastFall()
    {
        PlayerState = PlayerState.FastFall;
        physics.FastFall();
    }

    private void ResetSlideTimer()
    {
        slideTimeLeft = 0;
    }

}

public enum PlayerState
{
    Idle = 0,
    Run = 1, 
    Jump = 2, 
    FastFall = 3, 
    Slide = 4
}