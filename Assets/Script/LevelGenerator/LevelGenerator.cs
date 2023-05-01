using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GroundSide
{
	Right = 0,
	Top = 1,
	Left = 2
}

public class LevelGenerator : MonoBehaviour
{
	private delegate void ChangeSpriteFunc(Block block);

	public static System.Action<BlockBase> OnPassingBackEdge;

	[SerializeField] private Transform OutOfScreenPosition;

	[SerializeField]
	private Sprite ground;

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
	private float ChanceToSpawnEnemy = 0.3f;

	private ChangeSpriteFunc[] SetBlockSprite = new ChangeSpriteFunc[3];

	private int DistanceBetweenHugeDecoration = 2;

	[SerializeField] private int CurrentDistanceFromLastHugeDecoration = 0;

	public EnemyInfo[] EnemiesInfo;
	private int BaseMaxDistanceBetweenEnemies = 3;
	private int CurrentMaxDistanceBetweenEnemies;
	private int IncreasedDistancePer30Sec = 1;
	[SerializeField] private int CurrentDistanceFromLastEnemy = 0;

	private int MinBlockOfSameBiome = 50;
	private int CurrentBlocksOfSameBiome = 0;

	private int BlocksToNotPlaceAnythingInStart = 30;

	[SerializeField] private SpriteRenderer BackGround;

	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private LayerMask otherLayer;
	public void Start()
	{
		OnPassingBackEdge = null;


		SetBlockSprite[0] = SetLeftSprite;
		SetBlockSprite[1] = SetTopSprite;
		SetBlockSprite[2] = SetRightSprite;

		foreach (var block in blocks)
		{
			blocksPool.AddItem(block);
		}

		foreach(var decoration in hugeDecorations)
		{
			hugeDecorationPool.AddItem(decoration);
		}

		foreach(var enemyBlock in enemyBlocks)
		{
			enemiesPool.AddItem(enemyBlock);
		}

		CurrentMaxDistanceBetweenEnemies = BaseMaxDistanceBetweenEnemies;

		SwapTileSet();
		
		
		var newBlock = blocksPool.PickAvailableItem();
		newBlock.gameObject.transform.parent = world.transform;
		newBlock.gameObject.transform.position = startPoint.position;
		newBlock.Sprite.sprite = CurrentTileSet.GetRandomGround();
		lastBlock = newBlock;
		OnPassingBackEdge += OnBlockBecameInvisible;

		for (int i = 0; i < BlocksToNotPlaceAnythingInStart; i++)
		{
			SpawnGround(GroundSide.Top);
			SpawnDecor();
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
			enemiesPool.AddItem((EnemyBlock)block);
		}
	}

	public void FixedUpdate()
	{
		if (lastBlock.Transform.position.x < generataionEdge.position.x)
		{
			
			if (IsRandomTrue(ChanceToSetWater))
			{
				PlaceWater();
			}
			else
			{
				DefaultSpawn();
			}
			CurrentDistanceFromLastEnemy++;
			CurrentDistanceFromLastHugeDecoration++;
			CurrentBlocksOfSameBiome++;
		}
		
		if(CurrentBlocksOfSameBiome > MinBlockOfSameBiome)
		{
			if (IsRandomTrue(ChanceToChangeTileSet))
			{
				SwapTileSet();
			}
			else
			{
				CurrentBlocksOfSameBiome = 0;
			}
		}
	}

	private void DefaultSpawn()
	{
		SpawnGround(GroundSide.Top);

		if (IsRandomTrue(ChanceToSpawnEnemy))
		{
			if (CurrentDistanceFromLastEnemy > CurrentMaxDistanceBetweenEnemies)
			{
				SpawnEnemy();

				CurrentDistanceFromLastEnemy = -1;

				return;
			}
		}
		else
		{
			SpawnDecor();
		}
	}

	private void SpawnDecor()
	{
		if (IsRandomTrue(ChanceToSpawnDecoration))
		{
			SpawnSmallDecor();
		}
		else
		{

			if (CurrentDistanceFromLastHugeDecoration >= DistanceBetweenHugeDecoration && IsRandomTrue(ChanceToSpawnHugeDecoration))
			{
				SpawnHugeDecoration();
				CurrentDistanceFromLastHugeDecoration = -1;
			}
		}
	}

	private void SpawnEnemy()
	{
		Vector3 newBlockPosition = lastBlock.Transform.position + Vector3.up;

		var rndEnemyInfo = EnemiesInfo[Random.Range(0, EnemiesInfo.Length)];

		var newEnemy = enemiesPool.PickAvailableItem();
		newEnemy.Transform.position = newBlockPosition;
		newEnemy.Transform.parent = null;
		newEnemy.Transform.parent = world.transform;

		float chanceToBeInAir = 0.5f;

		if(rndEnemyInfo.GroundEntityType == EntityWalkType.Air && IsRandomTrue(chanceToBeInAir))
		{
			newEnemy.Transform.position = newEnemy.Transform.position  + Vector3.up * 0.5f;
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
		block.Sprite.sprite = CurrentTileSet.GroundRightEdge;
	}

	private void SetLeftSprite(Block block)
	{
		block.Sprite.sprite = CurrentTileSet.GroundLeftEdge;
	}

	private bool IsRandomTrue(float chance)
	{
		float rndNum = Random.Range(0f, 1f);
		return rndNum < chance;
	}

	private void PlaceWater()
	{
		SpawnGround(GroundSide.Top);
		SpawnDecor();
		SpawnGround(GroundSide.Top);
		SpawnSmallDecor();
		SpawnGround(GroundSide.Right);
		SpawnSmallDecor();

		int rnd = Random.Range(2, 3);
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
			lastBlock = newBlock;
		}
		SpawnGround(GroundSide.Left);
		SpawnSmallDecor();
		SpawnGround(GroundSide.Top);
		SpawnSmallDecor();
		SpawnGround(GroundSide.Top);
		SpawnDecor();
		CurrentDistanceFromLastEnemy = 0;
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