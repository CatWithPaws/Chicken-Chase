using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGameResetHandler : MonoBehaviour
{
    private void OnDestroy()
    {
        GameData.OnMoneyUpdate = null;
        GameData.OnSkinUpdate = null;
    }
}
