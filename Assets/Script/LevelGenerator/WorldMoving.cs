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

	[SerializeField] private float speed = 1.5f;

	public float PassedDistance { get; private set; }
	private float growingSpeedMultiplierPerSecond = 0.01f;


	private void FixedUpdate()
	{
		if (!player.IsPlayingGame) { return; }
		var children = transform.GetComponentsInChildren<Transform>();
		foreach(var obj in children)
		{
			obj.transform.Translate(Vector3.left*Time.fixedDeltaTime * speed);
		}
		if (speed < 15)
		{
			speed += Time.fixedDeltaTime * growingSpeedMultiplierPerSecond;
		}
		player.ChangeAnimationSpeed(speed/4);

		float distanceToAdd = speed * Time.fixedDeltaTime;


		PassedDistance += distanceToAdd;
	}
}
