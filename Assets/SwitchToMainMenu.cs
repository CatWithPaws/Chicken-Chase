using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchToMainMenu : MonoBehaviour
{
    [SerializeField] private Image blackScreeen;
    [SerializeField] private WorldMoving world;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Save.saveFile.IsFirstLauch = false;
            world.AllowedToMove = false;
            StartCoroutine(StartActualGame());
        }
    }


    private IEnumerator StartActualGame()
    {
        blackScreeen.DOFade(1f, 1f).OnComplete(() =>
        {
            blackScreeen.DOFade(1, 1).OnComplete(() =>
            {
                SceneManager.LoadScene(1);
            });
        });


        yield return null;
    }
}
