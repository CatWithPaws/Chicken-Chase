using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVerticalMovement : MonoBehaviour
{
    private Vector3 startStaticPosition;

	private Vector3 endStaticPosition;

    private Vector3 direction;

    public float Distance;

    public  float speed = 1;

	public bool canMove;

	public void RewriteCoords()
	{
		direction = Vector3.up;
		startStaticPosition = transform.position;
		endStaticPosition = transform.position + (direction * Distance);
	}

	private void FixedUpdate()
	{
		Vector3 endPositionWithEndY = new Vector3(transform.position.x, endStaticPosition.y, transform.position.z);
		transform.position = Vector3.Lerp(transform.position, endPositionWithEndY , speed * Time.fixedDeltaTime);
		if(Vector3.Distance(transform.position, endPositionWithEndY) < 0.1f)
		{
			var temp = endStaticPosition;
			endStaticPosition = startStaticPosition;
			startStaticPosition = endStaticPosition;
		}
	}

}
