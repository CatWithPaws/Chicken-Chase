using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
	public static int LayerMaskToLayer(LayerMask mask)
	{
		return Mathf.RoundToInt(Mathf.Log(mask.value, 2));
	}
}
