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

	[SerializeField] GameObject GameUI;
	public void StartGame()
	{
		StartCoroutine(StartScenario());
	}

	public IEnumerator StartScenario()
	{
		ChickenAnimator.Play("Run");
		while(Vector3.Distance(ChickenTransform.position,toChickenPosition.position) > 0.5f)
		{
			ChickenTransform.position = Vector3.Lerp(ChickenTransform.position, toChickenPosition.position, 0.5f*Time.fixedDeltaTime);
			yield return new WaitForFixedUpdate();
		}
		ChickenAnimator.Play("Idle");
		yield return new WaitForSeconds(2);
		ChickenAnimator.Play("Run");
		player.StartPlayer();
		GameUI.SetActive(true);
		while (Vector3.Distance(gameCamera.transform.position, ToCameraPosition.position) > 0.5f)
		{
			gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, ToCameraPosition.position, 0.5f * Time.fixedDeltaTime);
			yield return new WaitForFixedUpdate();
		}
	}
}
