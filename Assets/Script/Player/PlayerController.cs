using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Action OnPlayerDie;

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
            SwitchCollider();
            animations.SwitchAnimationTo(playerState);
        }
    }

    [SerializeField] private PhysicsComponent physics;
    [SerializeField] private AnimationComponent animations;

    private float slidingDuration = 0.5f;

    private float slideTimeLeft = 0f;

    public bool IsPlayingGame { get; private set; }

    [SerializeField] private CoinsUI coinsUI;

    private int maxAdditionalJumps = 1;
    private int currentAdditionalJump = 1;

	private void Awake()
	{
        GameData.Instance.Player = this;
        touchHandler.OnTouchMove += HandleTouch;
	}


    private void FixedUpdate()
    {
        if (IsPlayingGame)
        {
            CheckForSlideAfterFalling();
            CheckForSlideCoolDown();
            CheckForRunState();
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent(out Coin coin))
		{
            AddCoins(1);   
            LevelGenerator.OnPassingBackEdge(coin);
		}
	}

    public void AddCoins(int count)
    {
        if (GameData.Instance != null)
        {
            GameData.Instance.CoinsCount += count;
            coinsUI.UpdateCoins();
        }
        else
        {
            print("+Coin");
        }
	}

	public void ChangeAnimationSpeed(float newSpeed)
    {
        animations.ChangeSpeed(newSpeed);
    }

    public void OnEnemyTouch(EnemyBlock enemy)
    {
        if (IsPlayingGame)
        {
			Die();
		}
    }

    public void StartPlayer()
    {
		PlayerState = PlayerState.Run;
		IsPlayingGame = true;
        
    }
    public void Die()
    {
        IsAlive = false;
        IsPlayingGame = false;
		PlayerState = PlayerState.Idle;
        OnPlayerDie?.Invoke();
        GameData.Instance?.SaveCoins();
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
            Slide();
		}
	}

	private void CheckForRunState()
    {
        if(PlayerState == PlayerState.Jump)
        {
            if (groundChecker.isGrounded && physics.VerticalVelocity <= 0)
            {
                PlayerState = PlayerState.Run;
            }
        }
    }

	private void HandleTouch(Vector2 moveVector)
    {
        if (IsPlayingGame)
        {
            if (moveVector.y > 0)
            {
                OffSlideTimer();

                CheckForJump();
            }
            else if (moveVector.y < 0)
            {
                SlideOrFastFall();
            }
        }
    }

	private void CheckForJump()
    {
        if (groundChecker.isGrounded)
        {
            Jump();
        }
        else if(currentAdditionalJump > 0)
        {
            currentAdditionalJump--;
            Jump();
        }
    }


    private void Jump()
    {
		OffSlideTimer();
		PlayerState = PlayerState.Jump;
		physics.Jump();
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

    private void OffSlideTimer()
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