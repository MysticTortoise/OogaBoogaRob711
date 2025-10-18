using UnityEngine;
using System.Collections;

public class CameraVFX : MonoBehaviour
{
    public IEnumerator ScreenShake(float shakeDuration, float shakeMagnitude, Camera mainCamera)
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

    public void SetZoom(float value, Camera mainCamera)
    {
        mainCamera.orthographicSize = value;
    }
}
