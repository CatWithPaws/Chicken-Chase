using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour
{
    void Start()
    {
        GameData.Load();

        SceneManager.LoadScene(1);
    }
}
