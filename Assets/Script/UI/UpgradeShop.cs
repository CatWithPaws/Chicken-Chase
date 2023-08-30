using Delegates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeShop : MonoBehaviour
{
    public VoidFunc OnUpgradeBuy;


    [SerializeField] private BuffsInfo buffInfo;
    [SerializeField] private UpgradeCell[] cells;

    private void Awake()
    {
        OnUpgradeBuy = UpdateShop;
    }

    public void OnEnable()
    {
        UpdateShop();
    }

    public void UpdateShop()
    {
        int lastCellIndex = 0;

        foreach (var cell in cells)
        {
            cell.gameObject.SetActive(false);
        }

        foreach(var buff in buffInfo.Buffs)
        {
            cells[lastCellIndex].ShowCell(buff.BuffType, buff.Sprite);
            lastCellIndex++;
        }
    }
}
