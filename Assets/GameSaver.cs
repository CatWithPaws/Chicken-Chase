using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaver : MonoBehaviour
{
    private void OnApplicationPause(bool pause)
    {
        GameData.SaveGame();
    }

    private void OnApplicationQuit()
    {
        GameData.SaveGame();
    }

    private void OnDestroy()
    {
        GameData.SaveGame();
    }
}
