using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCounter : MonoBehaviour
{
	[SerializeField] LayerMask groundLayer;
	[SerializeField] LayerMask waterLayer;

	public long distanceInBlocks = 0;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.layer == Utility.LayerMaskToLayer(groundLayer) || collision.gameObject.layer == Utility.LayerMaskToLayer(waterLayer))
		{
			distanceInBlocks++;
		}
	}
}
