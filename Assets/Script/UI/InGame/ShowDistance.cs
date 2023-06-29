using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowDistance : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextDistance;
	[SerializeField] WorldMoving world;

	public void Update()
	{
		TextDistance.text = "Distance: " + Mathf.Floor(world.PassedDistance) + "ì";
	}
}
