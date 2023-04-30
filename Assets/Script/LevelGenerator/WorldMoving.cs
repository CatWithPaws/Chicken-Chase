using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

	private float growingSpeedMultiplierPerSecond = 0.01f;

	private void FixedUpdate()
	{
		var children = transform.GetComponentsInChildren<Transform>();
		foreach(var obj in children)
		{
			obj.transform.Translate(Vector3.left*Time.fixedDeltaTime * speed);
		}
		if (speed < 20)
		{
			speed += Time.fixedDeltaTime * growingSpeedMultiplierPerSecond;
		}
	}
}
