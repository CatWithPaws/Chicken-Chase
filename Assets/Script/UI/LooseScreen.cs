using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LooseScreen : MonoBehaviour
{
	[SerializeField] private GameObject InGameUI;
    [SerializeField] private GameObject loosePanel;
	[SerializeField] private WorldMoving world;

	[SerializeField] private TextMeshProUGUI passedDistance;

    [SerializeField] private TextMeshProUGUI bestDistance;

	[SerializeField] private GameObject newRecordText;
	[SerializeField] private GameObject youLooseText;

    private void Start()
	{
		GameData.Player.OnPlayerDie += OnPlayerDie;
	}

	private void OnPlayerDie()
	{
		InGameUI.SetActive(false);
		passedDistance.text = Mathf.Floor(world.PassedDistance) + " meters";
		
		bestDistance.text = GameData.GetBestDistance.ToString() + " meters";
        if (world.PassedDistance > GameData.GetBestDistance)
        {
            GameData.SetBestDistance((long)world.PassedDistance);
            youLooseText.SetActive(false);
            newRecordText.SetActive(true);
        }

        loosePanel.SetActive(true);
    }

	public void GoToMenu()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
