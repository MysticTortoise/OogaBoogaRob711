using System;
using UnityEngine;

public class KeepCameraWidth : MonoBehaviour
{
    [SerializeField] private float desiredAspectWidth;

    private Camera cam;
    private float initialZoom;
    private float ratioToDesired;

    private void Start()
    {
        cam = GetComponent<Camera>();
        initialZoom = cam.orthographicSize;
        ratioToDesired = desiredAspectWidth;
    }

    private void Update()
    {
        float ratio = Screen.width / (float)Screen.height;

        if (ratio < desiredAspectWidth)
        {
            ratioToDesired = desiredAspectWidth / ratio;
            cam.orthographicSize = initialZoom * ratioToDesired;
        }
        else
        {
            ratioToDesired = Mathf.Max(desiredAspectWidth / ratio, 1);
            cam.orthographicSize = initialZoom * ratioToDesired;
        }
    }
}
