using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraLook : MonoBehaviour
{
    [Tooltip("Speed of FOV changing")]
    public float speedFOV = 500f;
    [Tooltip("Speed of rotation")]
    public float speedRot = 300f;
    [Tooltip("Size of buffer zone")]
    public float pxThreshold = 50;
    [Tooltip("Size of main padding area")]
    public float pxPad = 150;

    float pxThresholdAbs;
    Vector2 threshold;
    Vector2 pad;
    Camera cam;
    UIHandler uiHandler;
    BoundingBox bbox;

    void Start()
    {
        // links
        cam = Camera.main;
        uiHandler = GetComponent<UIHandler>();
        bbox = GetComponent<BoundingBox>();
        
        // convert pixels to [0,1]
        pad.x = pxPad / Screen.width;
        pad.y = pxPad / Screen.height;
        pxThresholdAbs = pxThreshold + pxPad;
        threshold.x = pxThresholdAbs / Screen.width;
        threshold.y = pxThresholdAbs / Screen.height;

        // set images size
        uiHandler.SetPaddingArea(pxPad);
        uiHandler.SetThresholdArea(pxThresholdAbs);
    }

    void Update()
    {
        bbox.UpdateMargin();
        Vector2 center = GetCenter();

        // get angle
        float stepRotX = Math.Abs(center.x - 0.5f) * Time.deltaTime * speedRot;
        float stepRotY = Math.Abs(center.y - 0.5f) * Time.deltaTime * speedRot;
        
        // horizontal rotation
        if (center.x > 0.5) cam.transform.Rotate(Vector3.up * stepRotX, Space.World);
        else cam.transform.Rotate(-Vector3.up * stepRotX, Space.World);

        // vert rotation
        if (center.y > 0.5) cam.transform.Rotate(-Camera.main.transform.right * stepRotY, Space.World);
        else cam.transform.Rotate(Camera.main.transform.right * stepRotY, Space.World);

        // increase fov
        if (bbox.xmax > 1 - pad.x || bbox.xmin < 0 + pad.x || bbox.ymax > 1 - pad.y || bbox.ymin < 0 + pad.y)
        {
            float stepFOV = GetDistanceOutside() * Time.deltaTime * speedFOV; //(bbox.ymax - (1-pad.y)) 
            Camera.main.fieldOfView += stepFOV;
        }
        // decrease fov
        else if (bbox.xmax < 1 - threshold.x && bbox.xmin > 0 + threshold.x && bbox.ymax < 1 - threshold.y && bbox.ymin > 0 + threshold.y)
        {
            float stepFOV = GetDistanceInside() * Time.deltaTime * speedFOV; //(bbox.ymax - (1-pad.y)) 
            Camera.main.fieldOfView -= stepFOV;
        }
    }

    /// <summary> simple calculation of center in 2D space </summary>
    Vector2 GetCenter()
    {
        return new Vector2((bbox.xmax + bbox.xmin) / 2, (bbox.ymax + bbox.ymin) / 2);
    }

    /// <summary> returns maximum distance between padding area and objects </summary>
    float GetDistanceOutside() 
    {
        // calculate gaps
        float stepFovRight = bbox.xmax - (1 - pad.x);
        float stepFovLeft = (0 + pad.x) - bbox.xmin;
        float stepFovUp = bbox.ymax - (1 - pad.y);
        float stepFovDown = (0 + pad.y) - bbox.ymin;

        // get max
        float stepFovMax = stepFovRight;
        if (stepFovLeft > stepFovMax) stepFovMax = stepFovLeft;
        if (stepFovUp > stepFovMax) stepFovMax = stepFovUp;
        if (stepFovDown > stepFovMax) stepFovMax = stepFovDown;

        return stepFovMax;
    }

    /// <summary> returns minimum distance between threshold area and objects </summary>
    float GetDistanceInside()
    {
        // calculate gaps
        float stepFovRight = (1 - threshold.x) - bbox.xmax;
        float stepFovLeft = bbox.xmin - (0 + threshold.x);
        float stepFovUp = (1 - threshold.y) - bbox.ymax;
        float stepFovDown = bbox.ymin - (0 + threshold.y);

        // get min
        float stepFovMin = stepFovRight;
        if (stepFovLeft < stepFovMin) stepFovMin = stepFovLeft;
        if (stepFovUp < stepFovMin) stepFovMin = stepFovUp;
        if (stepFovDown < stepFovMin) stepFovMin = stepFovDown;

        return stepFovMin;
    }

}
