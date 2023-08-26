using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

enum GroundSide
{
	Right = 0,
	Top = 1,
	Left = 2
}

public class LevelGenerator : MonoBehaviour
{
	private delegate void VoidFunc();
	private delegate void ChangeSpriteFunc(Block block);

	public static System.Action<BlockBase> OnPassingBackEdge;

	[SerializeField] private Transform OutOfScreenPosition;

	[SerializeField]
	private WorldMoving world;

    [SerializeField]
    private List<Block> blocks = new List<Block>();

    [SerializeField]
    private List<HugeDecoration> hugeDecorations = new List<HugeDecoration>();

	[SerializeField]
	private List<EnemyBlock> enemyBlocks = new List<EnemyBlock>();

	[SerializeField]
	private TileSet[] Tilesets;
  
    private PoolObject<Block> blocksPool = new PoolObject<Block>();
	private PoolObject<HugeDecoration> hugeDecorationPool = new PoolObject<HugeDecoration>();
	private PoolObject<EnemyBlock> enemiesPool = new PoolObject<EnemyBlock>();

	[SerializeField]
	private Transform generataionEdge;
	
	[SerializeField]
	private Transform startPoint;
    
	private BlockBase lastBlock;

	private TileSet CurrentTileSet;

	private float ChanceToChangeTileSet = 0.2f;
	private float ChanceToSetWater = 0.05f;
	private float ChanceToSpawnDecoration = 0.3f;
	private float ChanceToSpawnHugeDecoration = 0.4f;
	private float ChanceToSpawnEnemy = 0.5f;

	private ChangeSpriteFunc[] SetBlockSprite = new ChangeSpriteFunc[3];

	private int DistanceBetweenHugeDecoration = 2;

	[SerializeField] private int currentDistanceFromLastHugeDecoration = 0;

	public EnemyInfo[] EnemiesInfo;
	private float currentMinDistanceBetweenEnemies = 4;
	[SerializeField] private int currentDistanceFromLastEnemy = 0;

	private int capOfMaxDistanceBetweenEnemies = 10;

	[SerializeField] private int DistanceLeftToAllowSpawnEnemy = 1;

	private int MinBlockOfSameBiome = 50;
	private int currentBlocksOfSameBiome = 0;

	private float currentMaxBlocksWater = 2;
	private float capOfMaxBlocksWater = 10;

	private int BlocksToNotPlaceAnythingInStart = 70;

	[SerializeField] private SpriteRenderer BackGround;

	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private LayerMask otherLayer;

	private float timerDurationInSeconds = 60;
	private float timerCounting = 0;

	[SerializeField] private List<CoinBlock> coins;
	private PoolObject<CoinBlock> coinPool = new PoolObject<CoinBlock>();

	private float chanceToSpawnCoin = 0.5f;

	[SerializeField] private Sprite coinSprite;

	[SerializeField] private PlayerController player;


	private int minDistanceBetweenBuff = 100;
	private int currentDistanceFromLastBuff = 0;

	private float chanceToSpawnBuff = 0.1f;

	private PoolObject<BuffBlock> buffPool = new PoolObject<BuffBlock>();


	[SerializeField] private WaterBlock[] waterBlocks;
	private PoolObject<WaterBlock> waterPool = new PoolObject<WaterBlock>();

	[SerializeField] private List<BuffBlock> buffs;
	[SerializeField] private Sprite magnetSprite;
	[SerializeField] private Sprite bridgeSprite;
	[SerializeField] private Sprite tripleJumpSprite;
	[SerializeField] private Sprite invincibilitySprite;


	float distanceToIncreaseRiverByOneBlock = 400;
	float distanceToIncreaseDistanceBetweenEnemies = 400;

	private Dictionary<BuffType,Buff> buffsInstances = new Dictionary<BuffType, Buff>();


	private void Awake() 
	{
        buffsInstances.Add(BuffType.TripleJump, new TripleJump(tripleJumpSprite));
		buffsInstances.Add(BuffType.Bridge,new Bridge(bridgeSprite));
		buffsInstances.Add(BuffType.Invincibility,new Invincibility(invincibilitySprite));

		BuffComponent.OnAddNewBuff += OnAddNewBuff;
		BuffComponent.OnRemoveBuff += OnRemoveBuff;
    }

	private void OnAddNewBuff(BuffType type)
	{
		if (type != BuffType.Bridge) return; 
		foreach(var water in waterBlocks)
		{
			water.OnBridgePowerUpActive();
		}
	}

	private void OnRemoveBuff(BuffType type)
	{
		if(type != BuffType.Bridge) return;
        foreach (var water in waterBlocks)
        {
            water.OnBridgePowerUpDisable();
        }
    }

    public void Start()
    {
        OnPassingBackEdge = null;
        InitSetSpriteFunction();

        LoadPools();

        SwapTileSet();
        BackGround.color = CurrentTileSet.BackgroundColor;
        CreateFirstGroundBlock();

        GenerateSafeArea();
    }

    private void CreateFirstGroundBlock()
    {
        var newBlock = blocksPool.PickAvailableItem();
        world.AddToWorld(newBlock);
        newBlock.gameObject.transform.position = startPoint.position;
        newBlock.Sprite.sprite = CurrentTileSet.GetRandomGround();
        lastBlock = newBlock;
        OnPassingBackEdge += OnBlockBecameInvisible;
    }

    private void GenerateSafeArea()
    {
        for (int i = 0; i < BlocksToNotPlaceAnythingInStart; i++)
        {
            SpawnGroundWithDecoration(GroundSide.Top);
            currentDistanceFromLastEnemy++;
            currentDistanceFromLastHugeDecoration++;
            currentBlocksOfSameBiome++;
        }
    }

    private void InitSetSpriteFunction()
    {
        SetBlockSprite[0] = SetLeftSprite;
        SetBlockSprite[1] = SetTopSprite;
        SetBlockSprite[2] = SetRightSprite;
    }

    private void LoadPools()
    {
        foreach (var block in blocks)
        {
            blocksPool.AddItem(block);
        }

        foreach (var decoration in hugeDecorations)
        {
            hugeDecorationPool.AddItem(decoration);
        }

        foreach (var enemyBlock in enemyBlocks)
        {
            enemiesPool.AddItem(enemyBlock);
        }

        foreach (var coin in coins)
        {
            coinPool.AddItem(coin);
        }

        foreach (var buff in buffs)
        {
            buffPool.AddItem(buff);
        }

		foreach(var water in waterBlocks)
		{
			waterPool.AddItem(water);
		}
    }


    private void OnBlockBecameInvisible(BlockBase block)
	{
		world.RemoveFromWorld(block);
		block.gameObject.layer = otherLayer;

		switch (block)
		{
			case Block:
                blocksPool.AddItem((Block)block);
                break;
			case HugeDecoration:
                hugeDecorationPool.AddItem((HugeDecoration)block);
				break;
			case EnemyBlock:
                EnemyBlock enemyBlock = (EnemyBlock)block;
                Destroy(enemyBlock.VerticalMovement);
                enemiesPool.AddItem(enemyBlock);
                break;
			case CoinBlock:
                coinPool.AddItem((CoinBlock)block);
                break;
			case BuffBlock:
				buffPool.AddItem((BuffBlock)block);
				break;
			case WaterBlock:
				waterPool.AddItem((WaterBlock)block);
				break;
        }
	}

	public void FixedUpdate()
	{

		ProcessTimer();
		if (lastBlock.Transform.position.x < generataionEdge.position.x)
        {
            DoGeneratorTick();
        }

        if (currentBlocksOfSameBiome > MinBlockOfSameBiome)
		{
			if (IsRandomTrue(ChanceToChangeTileSet))
			{
				SwapTileSet();
			}
			else
			{
				currentBlocksOfSameBiome = 0;
			}
		}
	}

    private void DoGeneratorTick()
    {
        if (IsRandomTrue(ChanceToSetWater))
        {
            PlaceWater();
        }
        else if (IsRandomTrue(chanceToSpawnBuff) && currentDistanceFromLastBuff >= minDistanceBetweenBuff)
        {
			if (!player.hasAnyBuff)
			{
				SpawnBuffPlace();
			}
        }
        else
        {
            DefaultSpawn();
        }


		currentMaxBlocksWater = Mathf.Clamp(currentMaxBlocksWater + (1 / distanceToIncreaseRiverByOneBlock), 0, capOfMaxBlocksWater);
	
		currentMinDistanceBetweenEnemies = Mathf.Clamp(currentMinDistanceBetweenEnemies + (1 / distanceToIncreaseDistanceBetweenEnemies), 0, capOfMaxDistanceBetweenEnemies);


        currentDistanceFromLastEnemy++;
        currentDistanceFromLastHugeDecoration++;
        currentBlocksOfSameBiome++;
		currentDistanceFromLastBuff++;
        if (DistanceLeftToAllowSpawnEnemy > 0)
        {
            DistanceLeftToAllowSpawnEnemy--;
        }
    }

    private void ProcessTimer()
	{
		timerCounting += Time.fixedDeltaTime;
		if(timerCounting >= timerDurationInSeconds)
		{
			OnTimerTick();
			timerCounting = 0;
		}
	}

	private void OnTimerTick()
	{
		currentDistanceFromLastEnemy++;
		currentMaxBlocksWater++;
	}

	private void DefaultSpawn()
	{
		SpawnGround(GroundSide.Top);


		if (IsRandomTrue(ChanceToSpawnEnemy))
		{
			if (currentDistanceFromLastEnemy >= currentMinDistanceBetweenEnemies)
			{
				TrySpawnEnemy();

				currentDistanceFromLastEnemy = -1;

				return;
			}
		}
		else
		{
			TrySpawnDecor();
			TrySpawnCoin();
		}

	}

	private void SpawnBuffPlace()
	{
		int DistanceBeforeAndAfterBuff = 5;

        for(int i = 0; i < DistanceBeforeAndAfterBuff; i++)
		{
            SpawnGroundWithDecoration(GroundSide.Top);
        }

        SpawnGroundWithDecoration(GroundSide.Top);
        SpawnBuff();

        for (int i = 0; i < DistanceBeforeAndAfterBuff; i++)
        {
            SpawnGroundWithDecoration(GroundSide.Top);
        }
    }

	private void SpawnGroundWithDecoration(GroundSide groundSide)
	{
        SpawnGround(groundSide);
        TrySpawnDecor();

    }

	private void SpawnBuff()
	{
		BuffBlock buff = (BuffBlock)SpawnObjectInTheWorld(buffPool);
		buff.Transform.Translate(Vector2.up*2);
		BuffType rndType = (BuffType)Random.Range(0, (int)BuffType.Count);
		buff.Buff = GetBuffByType(rndType).Clone();
        buff.Sprite.sprite = buff.Buff.Icon;

    }

	private Buff GetBuffByType(BuffType type)
	{
		try
		{
			return buffsInstances[type];
		}
		catch
		{
			Debug.LogError("BuffNotFound");
			return null;
		}
	}

	private void TrySpawnDecor()
	{
		if (IsRandomTrue(ChanceToSpawnDecoration))
		{
			SpawnSmallDecor();
		}


		else if (currentDistanceFromLastHugeDecoration >= DistanceBetweenHugeDecoration && IsRandomTrue(ChanceToSpawnHugeDecoration))
		{
			SpawnHugeDecoration();
			currentDistanceFromLastHugeDecoration = -1;
		}

	}
	
	private void TrySpawnCoin()
	{
		if (IsRandomTrue(chanceToSpawnCoin))
		{
			
			var coin = SpawnObjectInTheWorld(coinPool);
			coin.Sprite.sprite = coinSprite;
			coin.transform.position += Vector3.up * Random.Range(1,3);
			DistanceLeftToAllowSpawnEnemy = 1;
        }
	}

	private void TrySpawnEnemy()
	{
		if(DistanceLeftToAllowSpawnEnemy > 0) { return; }
		Vector3 newBlockPosition = lastBlock.Transform.position + Vector3.up;

		var rndEnemyInfo = EnemiesInfo[Random.Range(0, EnemiesInfo.Length)];

		EnemyBlock newEnemy = SpawnObjectInTheWorld(enemiesPool,newBlockPosition);

		float chanceToBeInAir = 0.5f;
		float chanceToBeMovableInAir = 0.5f;

		float minDistance = 0.5f;
		float maxDistance = 1f;

        newEnemy.Animator.runtimeAnimatorController = rndEnemyInfo.Animator;

        if (rndEnemyInfo.GroundEntityType == EntityWalkType.Air && IsRandomTrue(chanceToBeInAir))
		{
			if (IsRandomTrue(chanceToBeMovableInAir))
			{
				var verticalMovementComponent = newEnemy.gameObject.AddComponent<EnemyVerticalMovement>();
				verticalMovementComponent.Distance = Random.Range(minDistance, maxDistance);
				newEnemy.Transform.position = new Vector3(newBlockPosition.x, Random.Range(newBlockPosition.y, newBlockPosition.y + verticalMovementComponent.Distance), newBlockPosition.z);
				verticalMovementComponent.RewriteCoords();
				newEnemy.VerticalMovement = verticalMovementComponent;
			}
			else
			{
				newEnemy.Transform.position = newEnemy.Transform.position + Vector3.up * 0.5f;
			}
		}

		
		newEnemy.Collider.isTrigger = true;


	}


	private void SwapTileSet()
	{
		CurrentTileSet = Tilesets[Random.Range(0, Tilesets.Length)];
		StartCoroutine(ChangeBG(CurrentTileSet.BackgroundColor));
	}

	private IEnumerator ChangeBG(Color endColor)
	{
		BackGround.color =  Color.Lerp(BackGround.color, endColor, 6 * Time.deltaTime);
		yield return new WaitForSeconds(Time.deltaTime);
		if(BackGround.color == endColor)
		{
			StopCoroutine(ChangeBG(endColor));
		}
	}

	private T SpawnObjectInTheWorld<T>(PoolObject<T> pool, Vector3 position = default) where T : BlockBase
	{
		position = position == default ? lastBlock.transform.position : position;
		var newBlock = pool.PickAvailableItem();
        world.AddToWorld(newBlock);
        newBlock.Transform.position = position;
		return newBlock;
	}

	private void SpawnSmallDecor()
	{
		Block newDecoration = (Block)SpawnObjectInTheWorld(blocksPool);
		newDecoration.Transform.position = newDecoration.Transform.position + Vector3.up;
		newDecoration.Sprite.sprite = CurrentTileSet.GetRandomDecoration();
		newDecoration.Collider.isTrigger = true;
	}

	private void SpawnHugeDecoration()
	{
		HugeDecoration newHugeDecoration = (HugeDecoration)SpawnObjectInTheWorld(hugeDecorationPool);
		newHugeDecoration.Transform.position = newHugeDecoration.Transform.position + Vector3.up * 0.5f;
		newHugeDecoration.Sprite.sprite = CurrentTileSet.GetRandomHugeDecoration();
	}

	private void SpawnGround(GroundSide side)
	{
		Block newBlock = (Block)SpawnObjectInTheWorld(blocksPool);
		newBlock.Transform.position = newBlock.Transform.position + Vector3.right;
		SetBlockSprite[(int)side](newBlock);
		int layer = (Utility.LayerMaskToLayer(groundLayer));
		newBlock.gameObject.layer = layer;

		lastBlock = newBlock;
	}

	private void SetTopSprite(Block block)
	{
		block.Sprite.sprite = CurrentTileSet.GetRandomGround();
	}

	private void SetRightSprite(Block block)
	{
		block.Sprite.sprite = CurrentTileSet.GroundLeftEdge;
	}

	private void SetLeftSprite(Block block)
	{
		block.Sprite.sprite = CurrentTileSet.GroundRightEdge;
	}

	private bool IsRandomTrue(float chance)
	{
		float rndNum = Random.Range(0f, 1f);
		return rndNum < chance;
	}

	private void PlaceWater()
	{
		SpawnGround(GroundSide.Top);
		TrySpawnDecor();
		SpawnGround(GroundSide.Top);
		SpawnSmallDecor();
		SpawnGround(GroundSide.Right);
		SpawnSmallDecor();

		int rnd = Random.Range(1, (int)currentMaxBlocksWater+1);
		Sprite rndWater = CurrentTileSet.GetRandomWater();
		for (int i = 0; i < rnd; i++)
		{
			Vector3 newBlockPosition = lastBlock.Transform.position + Vector3.right;
			var newBlock = SpawnObjectInTheWorld<WaterBlock>(waterPool,newBlockPosition);
			newBlock.Sprite.sprite = rndWater;
			newBlock.gameObject.layer = LayerMask.NameToLayer("Water");
			
			lastBlock = newBlock;
		}
		SpawnGround(GroundSide.Left);
		SpawnSmallDecor();
		SpawnGround(GroundSide.Top);
		SpawnSmallDecor();
		SpawnGround(GroundSide.Top);
		TrySpawnDecor();
		currentDistanceFromLastEnemy = 0;
	}
}

public enum EntityWalkType
{
	Ground,Air
}

[System.Serializable]
public class EnemyInfo
{
	public RuntimeAnimatorController Animator;
	public EntityWalkType GroundEntityType;
}