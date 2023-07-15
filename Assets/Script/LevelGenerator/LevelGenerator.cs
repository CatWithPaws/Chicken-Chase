using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
	private GameObject world;

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
    
	private Block lastBlock;

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
	private int BaseMinDistanceBetweenEnemies = 4;
	private int CurrentMinDistanceBetweenEnemies = 4;
	[SerializeField] private int currentDistanceFromLastEnemy = 0;

	[SerializeField] private int DistanceLeftToAllowSpawnEnemy = 1;

	private int MinBlockOfSameBiome = 50;
	private int currentBlocksOfSameBiome = 0;

	private int maxBlocksWater = 2;

	private int BlocksToNotPlaceAnythingInStart = 30;

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


	private int minDistanceBetweenBuff = 50;
	private int currentDistanceFromLastBuff = 0;

	private float chanceToSpawnBuff = 0.1f;

	private PoolObject<BuffBlock> buffPool = new PoolObject<BuffBlock>();

	[SerializeField] private List<BuffBlock> buffs;
	[SerializeField] private Sprite magnetSprite;
	[SerializeField] private Sprite bridgeSprite;
	[SerializeField] private Sprite tripleJumpSprite;
	[SerializeField] private Sprite invincibilitySprite;

	[SerializeField] private Dictionary<BuffType, Sprite> spritesByBuffTypeList = new Dictionary<BuffType, Sprite>();
	private (BuffType, Buff)[] buffsInstances = new (BuffType, Buff)[]
	{
		(BuffType.TripleJump,new TripleJump()),
		(BuffType.Invincibility, new Invincibility())
	};
	private void Awake()
	{ 
	}

    public void Start()
    {
        OnPassingBackEdge = null;
        InitSetSpriteFunction();

        LoadPools();

		spritesByBuffTypeList.Add(BuffType.TripleJump, tripleJumpSprite);
		spritesByBuffTypeList.Add(BuffType.Invincibility, invincibilitySprite);

        CurrentMinDistanceBetweenEnemies = BaseMinDistanceBetweenEnemies;

        SwapTileSet();
        BackGround.color = CurrentTileSet.BackgroundColor;
        CreateFirstGroundBlock();

        GenerateSafeArea();
    }

    private void CreateFirstGroundBlock()
    {
        var newBlock = blocksPool.PickAvailableItem();
        newBlock.gameObject.transform.parent = world.transform;
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
    }


    private void OnBlockBecameInvisible(BlockBase block)
	{
		block.Transform.parent = null;
		block.Sprite.sprite = null;
		block.Transform.position = OutOfScreenPosition.position;
		block.gameObject.layer = otherLayer;

		if (block is Block)
		{
			blocksPool.AddItem((Block)block);
		}
		else if(block is HugeDecoration)
		{
			hugeDecorationPool.AddItem((HugeDecoration)block);
		}
		else if(block is EnemyBlock)
		{
			EnemyBlock enemyBlock = (EnemyBlock)block;
			Destroy(enemyBlock.VerticalMovement);
			enemiesPool.AddItem(enemyBlock);
		}
		else if(block is CoinBlock)
		{
            coinPool.AddItem((CoinBlock)block);

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
				print("bufff");
				SpawnBuffPlace();
			}
        }
        else
        {
            DefaultSpawn();
        }

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
		maxBlocksWater++;
	}

	private void DefaultSpawn()
	{
		SpawnGround(GroundSide.Top);


		if (IsRandomTrue(ChanceToSpawnEnemy))
		{
			if (currentDistanceFromLastEnemy >= CurrentMinDistanceBetweenEnemies)
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
		buff.Sprite.sprite = spritesByBuffTypeList[rndType];
		buff.Buff = GetBuffByType(rndType);
		
	}

	private Buff GetBuffByType(BuffType type)
	{
		foreach(var item in buffsInstances)
		{
			if (item.Item1 == type)
			{
				return item.Item2;
			}
		}
		Debug.LogError("There is no buff with type: " + type.ToString());
		return default;
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

		var newEnemy = enemiesPool.PickAvailableItem();
		newEnemy.Transform.position = newBlockPosition;
		newEnemy.Transform.parent = null;
		newEnemy.Transform.parent = world.transform;

		float chanceToBeInAir = 0.5f;
		float chanceToBeMovableInAir = 0.5f;

		float minDistance = 0.5f;
		float maxDistance = 1f;
		if (rndEnemyInfo.GroundEntityType == EntityWalkType.Air && IsRandomTrue(chanceToBeInAir))
		{
			if (IsRandomTrue(chanceToBeMovableInAir))
			{
				var verticalMovementComponent = newEnemy.gameObject.AddComponent<EnemyVerticalMovement>();
				verticalMovementComponent.Distance = Random.Range(minDistance, maxDistance);
				newEnemy.Transform.position = new Vector3(newBlockPosition.x, Random.Range(newBlockPosition.y, newBlockPosition.y + verticalMovementComponent.Distance), newBlockPosition.z);
				verticalMovementComponent.RewriteCoords();
				newEnemy.Type = rndEnemyInfo.Type;
				newEnemy.VerticalMovement = verticalMovementComponent;
			}
			else
			{
				newEnemy.Transform.position = newEnemy.Transform.position + Vector3.up * 0.5f;
			}
		}

		newEnemy.Animator.runtimeAnimatorController = rndEnemyInfo.Animator;
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

	private BlockBase SpawnObjectInTheWorld<T>(PoolObject<T> pool) where T : BlockBase
	{
		Vector3 newBlockPosition = lastBlock.Transform.position;
		var newBlock = pool.PickAvailableItem();
		newBlock.Transform.position = newBlockPosition;
		newBlock.Transform.parent = null;
		newBlock.Transform.parent = world.transform;
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

		int rnd = Random.Range(1, maxBlocksWater+1);
		Sprite rndWater = CurrentTileSet.GetRandomWater();
		for (int i = 0; i < rnd; i++)
		{
			Vector3 newBlockPosition = lastBlock.Transform.position + Vector3.right;
			var newBlock = blocksPool.PickAvailableItem();
			newBlock.Transform.position = newBlockPosition;
			newBlock.Transform.parent = null;
			newBlock.Transform.parent = world.transform;
			newBlock.Sprite.sprite = rndWater;
			newBlock.Collider.isTrigger = true;
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
	public EnemyType Type;
	public RuntimeAnimatorController Animator;
	public EntityWalkType GroundEntityType;
}