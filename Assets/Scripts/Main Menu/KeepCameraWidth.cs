using System;
using UnityEngine;

public class KeepCameraWidth : MonoBehaviour
{
    [SerializeField] private float desiredAspectWidth;

    private Camera cam;
    private float initialZoom;

    private void Start()
    {
        cam = GetComponent<Camera>();
        initialZoom = cam.orthographicSize;
    }

    private void Update()
    {
        float ratio = Screen.width / (float)Screen.height;
        float ratioToDesired = Mathf.Max(desiredAspectWidth / ratio, 1); // Clamp when below 1
        cam.orthographicSize = initialZoom * ratioToDesired;
    }
}
