using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    Renderer[] objects;
    Camera cam;
    [HideInInspector] public float xmax = 0.5f, xmin = 0.5f, ymax = 0.5f, ymin = 0.5f;

    void Start()
    {
        cam = Camera.main;
        objects = GameObject.Find("MovingObjects").GetComponentsInChildren<Renderer>();
    }

    /// <summary> returns coordinates of bounding box around all objects </summary>
    public void UpdateMargin()
    {
        Vector3 pos1 = cam.WorldToViewportPoint(objects[0].transform.position);
        xmax = pos1.x;
        xmin = pos1.x;
        ymax = pos1.y;
        ymin = pos1.y;

        foreach (Renderer obj in objects)
        {

            Vector3[] points = new Vector3[8];
            points[0] = obj.bounds.min;
            points[1] = obj.bounds.max;
            points[2] = new Vector3(points[0].x, points[0].y, points[1].z);
            points[3] = new Vector3(points[0].x, points[1].y, points[0].z);
            points[4] = new Vector3(points[1].x, points[0].y, points[0].z);
            points[5] = new Vector3(points[0].x, points[1].y, points[1].z);
            points[6] = new Vector3(points[1].x, points[0].y, points[1].z);
            points[7] = new Vector3(points[1].x, points[1].y, points[0].z);

            foreach (Vector3 point in points)
            {
                Vector3 pos = cam.WorldToViewportPoint(point);
                if (pos.x > xmax) xmax = pos.x;
                if (pos.x < xmin) xmin = pos.x;
                if (pos.y > ymax) ymax = pos.y;
                if (pos.y < ymin) ymin = pos.y;
            }
            //DrawBBox(points);
        }
    }

    /// <summary> visualize bounding box </summary>
    void DrawBBox(Vector3[] points)
    {
        Color lineColor = Color.green;
        Debug.DrawLine(points[5], points[1], lineColor);
        Debug.DrawLine(points[1], points[7], lineColor);
        Debug.DrawLine(points[7], points[3], lineColor);
        Debug.DrawLine(points[3], points[5], lineColor);
        Debug.DrawLine(points[2], points[6], lineColor);
        Debug.DrawLine(points[6], points[4], lineColor);
        Debug.DrawLine(points[4], points[0], lineColor);
        Debug.DrawLine(points[0], points[2], lineColor);
        Debug.DrawLine(points[5], points[2], lineColor);
        Debug.DrawLine(points[1], points[6], lineColor);
        Debug.DrawLine(points[7], points[4], lineColor);
        Debug.DrawLine(points[3], points[0], lineColor);
    }

}
