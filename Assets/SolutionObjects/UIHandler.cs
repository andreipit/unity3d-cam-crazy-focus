using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    
    /// <summary> change size of image PaddingArea </summary>
    public void SetPaddingArea(float pxPad)
    {
        RectTransform padArea = GameObject.Find("PaddingArea").GetComponent<RectTransform>();
        padArea.SetLeft(pxPad);
        padArea.SetRight(pxPad);
        padArea.SetTop(pxPad);
        padArea.SetBottom(pxPad);
    }

    /// <summary> change size of image ThresholdArea </summary>
    public void SetThresholdArea(float pxThresholdAbs)
    {
        RectTransform thrArea = GameObject.Find("ThresholdArea").GetComponent<RectTransform>();
        thrArea.SetLeft(pxThresholdAbs);
        thrArea.SetRight(pxThresholdAbs);
        thrArea.SetTop(pxThresholdAbs);
        thrArea.SetBottom(pxThresholdAbs);
    }

}
