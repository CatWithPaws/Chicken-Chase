using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlicePanel : Image
{
    private void Update()
    {
        pixelsPerUnitMultiplier = Camera.main.aspect;
    }

}
