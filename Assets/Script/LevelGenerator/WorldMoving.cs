using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class WorldMoving : MonoBehaviour
{
	public float SpeedMultiplier
	{
		get
		{
			return speedMultiplier;
		}
	}
	[SerializeField]
	private float speedMultiplier = 1.5f;
	private float growingSpeedMultiplierPerMinute = 0.01f;
	private void FixedUpdate()
	{
		var children = transform.GetComponentsInChildren<Transform>();
		foreach(var obj in children)
		{
			obj.transform.Translate(Vector3.left*Time.fixedDeltaTime * speedMultiplier);
		}
		if (speedMultiplier < 20)
		{
			speedMultiplier += Time.fixedDeltaTime * growingSpeedMultiplierPerMinute;
		}
	}
}
