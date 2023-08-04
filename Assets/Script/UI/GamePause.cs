using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TextMeshProUGUI panelText;

    [SerializeField] private GameObject inGameUI;


    private int timeAfterResumingToStartAGame = 3;

    private void Start()
    {
        
    }


    private void PauseGame()
    {
        EnablePauseMenu();
        Time.timeScale = 0f;
    }

    private void EnablePauseMenu()
    {
        panelText.gameObject.SetActive(true);
        inGameUI.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void StartPause()
    {
        PauseGame();
    }

    public void StopPause()
    {
        StartCoroutine(StartResumeGame());
    }

    IEnumerator StartResumeGame()
    {
        pausePanel.SetActive(false);
        for(int i = timeAfterResumingToStartAGame; i > 0; i--)
        {
            panelText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
        }
        ResumeGame();
    }

    private void ResumeGame()
    {
        DisablePauseMenu();
        Time.timeScale = 1f;
    }

    private void DisablePauseMenu()
    {
        inGameUI.SetActive(true);
        panelText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;
    }
}
