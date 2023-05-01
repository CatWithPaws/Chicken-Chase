using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowDistance : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextDistance;
	[SerializeField] private WorldMoving world;

	public void Update()
	{
		TextDistance.text = "Distance:" + Mathf.Ceil(world.PassedDistance) + "m";
	}
}
