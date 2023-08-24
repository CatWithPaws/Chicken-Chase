using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowRecord : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI record;

    private void Awake()
    {
        record.text = GameData.GetBestDistance.ToString() + " m";
    }
}
