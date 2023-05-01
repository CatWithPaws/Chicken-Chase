using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LooseScreen : MonoBehaviour
{
    [SerializeField] private GameObject loosePanel;
	[SerializeField] private WorldMoving world;

	[SerializeField] private TextMeshProUGUI passedDistance;

	private void Start()
	{
		GameData.Instance.Player.OnPlayerDie += OnPlayerDie;
	}

	private void OnPlayerDie()
	{
		passedDistance.text = Mathf.Floor(world.PassedDistance) + " метрів";
		loosePanel.SetActive(true);
	}

	public void GoToMenu()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
