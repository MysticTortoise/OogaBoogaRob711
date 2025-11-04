using System;
using UnityEngine;

public class DisableAfter : MonoBehaviour
{
    public float timeTillGone;
    private float timer;
    private void OnEnable()
    {
        timer = timeTillGone;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
