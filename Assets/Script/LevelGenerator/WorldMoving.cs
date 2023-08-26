using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class WorldMoving : MonoBehaviour
{

	[SerializeField] private PlayerController player;

	public float Speed
	{
		get
		{
			return speed;
		}
	}

	[SerializeField] private float speed = 5f;

	[SerializeField] private Transform usedBlocksTransform;

	public float PassedDistance { get; private set; }
	private float growingSpeedMultiplierPerSecond = 0.03f;

	private readonly List<BlockBase> blocks = new List<BlockBase>();

    private void FixedUpdate()
	{
		if (!player.IsPlayingGame) { return; }
		

		foreach(var obj in blocks)
		{
			obj.Transform.Translate(Vector3.left*Time.fixedDeltaTime * speed);
		}
		if (speed < 15)
		{
			speed += Time.fixedDeltaTime * growingSpeedMultiplierPerSecond;
		}
		player.ChangeAnimationSpeed(speed/4);

		float distanceToAdd = speed * Time.fixedDeltaTime;


		PassedDistance += distanceToAdd;
	}

	public void AddToWorld(BlockBase block)
	{
		block.Transform.parent = this.transform;
		blocks.Add(block);
	}

	public void RemoveFromWorld(BlockBase block)
	{
		block.transform.parent = usedBlocksTransform;
		block.transform.localPosition = Vector3.zero;
        blocks.Remove(block);
	}

}
