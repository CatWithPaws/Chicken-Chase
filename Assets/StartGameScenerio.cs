using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScenerio : MonoBehaviour
{
    [SerializeField] Camera gameCamera;
    [SerializeField] Transform ChickenTransform;
	[SerializeField] Transform toChickenPosition;
    [SerializeField] Animator ChickenAnimator;

	[SerializeField] Transform ToCameraPosition;

	[SerializeField] PlayerController player;

	private void Start()
	{
		StartCoroutine(StartScenario());
	}

	public IEnumerator StartScenario()
	{
		while(Vector3.Distance(ChickenTransform.position,toChickenPosition.position) > 0.5f)
		{
			ChickenTransform.position = Vector3.Lerp(ChickenTransform.position, toChickenPosition.position, 0.1f*Time.fixedDeltaTime);
			yield return new WaitForFixedUpdate();
		}

		player.IsPlayingGame = true;
		yield return new WaitForSeconds(1);
	}
}
