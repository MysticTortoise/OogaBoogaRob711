using UnityEngine;
using System.Collections;

public class CameraVFX : MonoBehaviour
{
    private float goalZoom;
    private Camera mainCamera;

    [SerializeField] private float zoomLerpSpeed = 1;

    private void Start()
    {
        mainCamera = FindAnyObjectByType<Camera>();
        SetZoom(mainCamera.orthographicSize);
    }

    private void Update()
    {
        mainCamera.orthographicSize =
            MysticUtil.Damp(mainCamera.orthographicSize, goalZoom, zoomLerpSpeed, Time.deltaTime);
    }
    public IEnumerator ScreenShake(float shakeDuration, float shakeMagnitude)
    {
        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }

    public void StartScreenShake(float duration, float magnitude)
    {
        StartCoroutine(ScreenShake(duration, magnitude));
    }

    public void SetZoom(float value)
    {
        mainCamera.orthographicSize = value;
        goalZoom = value;
    }

    public void PunchZoom(float value)
    {
        mainCamera.orthographicSize = value;
    }
}
