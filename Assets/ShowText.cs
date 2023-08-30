using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShowText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [TextArea]
    [SerializeField] string content;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        print(content);

        if (collision.gameObject.CompareTag("Player"))
        {
            text.text = content;
            GetComponent<Collider2D>().enabled = false;
            text.DOFade(1, 0.5f).OnComplete(() =>
            {
                text.DOFade(1, 5f).OnComplete(() =>
                {
                    text.DOFade(0, 0.5f);
                });
            });
        }
    }

}
