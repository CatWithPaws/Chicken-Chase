using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackEdge : MonoBehaviour
{
	private string blockTag = "Block";
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent(out BlockBase block))
		{
			if(block is Block)
			{
				((Block)block).Collider.isTrigger = false;
			}
			LevelGenerator.OnPassingBackEdge.Invoke(block);
		}
	}
}
