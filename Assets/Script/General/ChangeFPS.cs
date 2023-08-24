using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFPS : MonoBehaviour
{
	private void Awake()
	{
		Application.targetFrameRate = 60;
	}
}
