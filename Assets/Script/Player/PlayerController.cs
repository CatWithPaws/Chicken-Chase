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

    [SerializeField] private BoxCollider2D defaultCollider;
    [SerializeField] private BoxCollider2D slideCollider;

    [SerializeField] private BoxCollider2D playerCollider;

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
    [SerializeField] private BuffComponent buffs;

    private float slidingDuration = 0.5f;

    private float slideTimeLeft = 0f;

    public bool IsPlayingGame { get; private set; }

    [SerializeField] private CoinsUI coinsUI;

    private int baseAdditionalJumps = 1;
    private int modificatorOfAdditionalJumps = 0;
    private int finalAdditionalJumps => baseAdditionalJumps + modificatorOfAdditionalJumps;

    private int currentAdditionalJump = 1;

    [SerializeField] private AudioClip coinPickSound;
    [SerializeField] private AudioClip firstJumpSound;
	[SerializeField] private AudioClip secondJumpSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip drowningSound;
    [SerializeField] private AudioClip slideSound;

    public bool IsInvincible = false;

    public bool hasAnyBuff => buffs.Buffs.Count > 0;

    private Vector2 startPosition;

	private void Awake()
	{
        GameData.Player = this;
        touchHandler.OnTouchMove += HandleTouch;
        startPosition = transform.position;
	}

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HandleTouch(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HandleTouch(Vector2.down);
        }
    }
#endif

    private void FixedUpdate()
    {
        if (IsPlayingGame)
        {
            CheckForSlideAfterFalling();
            CheckForSlideCoolDown();
            CheckForRunState();
            CheckForResetJumps();
            CheckForCorrectXPositioning();
        }
    }

    private void CheckForCorrectXPositioning()
    {
        if(transform.position.x != startPosition.x)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(startPosition.x, transform.position.y),Time.fixedDeltaTime * 2);
        }
    }

    private void CheckForResetJumps()
	{
        if (groundChecker.lastIsGrounded != groundChecker.isGrounded && groundChecker.isGrounded == true)
        {
            currentAdditionalJump = finalAdditionalJumps;
        }
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent(out CoinBlock coinBlock))
		{
            AddCoins(1);   
            LevelGenerator.OnPassingBackEdge(coinBlock);
		}
        if (collision.gameObject.TryGetComponent(out BuffBlock buffBlock))
        {
            buffs.AddBuff(buffBlock.Buff);
            LevelGenerator.OnPassingBackEdge(buffBlock);
        }
    }

    public void AddCoins(int count)
    {
        GameData.CoinsCount += count;


        AudioManager.Instance?.PlaySFX(coinPickSound);
    }

	public void ChangeAnimationSpeed(float newSpeed)
    {
        animations.ChangeSpeed(newSpeed);
    }

    public void OnEnemyTouch(EnemyBlock enemy)
    {
        if (IsPlayingGame && !IsInvincible)
        {
			DieFromEnemy();
		}
    }

    public void StartPlayer()
    {
		PlayerState = PlayerState.Run;
		IsPlayingGame = true;
        
    }

    public void DieFromDrowning()
    {
        if (IsPlayingGame)
        {
            AudioManager.Instance?.PlaySound(drowningSound);
            Die();
        }
    }

    public void DieFromEnemy()
    {
        if (IsPlayingGame & !IsInvincible)
        {
            AudioManager.Instance?.PlaySound(deathSound);
            Die();
        }
    }

   private void Die()
    {
		IsAlive = false;
		IsPlayingGame = false;
		PlayerState = PlayerState.Idle;
        buffs.RemoveAllBuffs();
		OnPlayerDie?.Invoke();
		GameData.SaveCoins();
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
            AudioManager.Instance?.PlaySound(firstJumpSound);
        }
        else if(currentAdditionalJump > 0)
        {
            currentAdditionalJump--;
            Jump();

			AudioManager.Instance?.PlaySound(secondJumpSound);
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
            Slide();
			AudioManager.Instance?.PlaySound(slideSound);
		}
        else
        {
            FastFall();
			AudioManager.Instance?.PlaySound(slideSound);
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
        playerCollider.size = playerState == PlayerState.Slide ? slideCollider.size: defaultCollider.size;
		playerCollider.offset = playerState == PlayerState.Slide ? slideCollider.offset : defaultCollider.offset;
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

    public void AddAdditionalJumps(int value)
    {
        modificatorOfAdditionalJumps += value;
    }

    public void SubstractAdditionalJumps(int value)
    {
        modificatorOfAdditionalJumps -= value;
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