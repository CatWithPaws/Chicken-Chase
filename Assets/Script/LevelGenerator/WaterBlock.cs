using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBlock : BlockBase
{
    [SerializeField] private GameObject bridge;

    public void OnBridgePowerUpActive()
    {
        bridge.SetActive(true);
    }

    public void OnBridgePowerUpDisable()
    {
        bridge.SetActive(false);
    }

}
