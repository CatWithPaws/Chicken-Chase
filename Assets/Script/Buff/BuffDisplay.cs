using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffDisplay : MonoBehaviour
{

    [SerializeField] private BuffIndicator[] indicators;
    [SerializeField] private BuffComponent playerBuffComponent;

    private void Awake()
    {
        foreach (var indicator in indicators)
        {
            indicator.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < indicators.Length; i++)
        {
            indicators[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < indicators.Length && i < playerBuffComponent.Buffs.Count; i++)
        {
            indicators[i].gameObject.SetActive(true);
            Buff currentBuff = playerBuffComponent.Buffs[i];
            float fillCoeficient = currentBuff.Duration / currentBuff.BaseDuration;

            indicators[i].BuffIcon.sprite = currentBuff.Icon;

            indicators[i].BuffFill.fillAmount = fillCoeficient;
        }
    }

}
