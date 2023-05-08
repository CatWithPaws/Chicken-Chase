using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowDistance : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextDistance;
	[SerializeField] DistanceCounter distanceCounter;

	public void Update()
	{
		TextDistance.text = "³�����: " + Mathf.Floor(distanceCounter.distanceInBlocks) + "�";
	}
}
