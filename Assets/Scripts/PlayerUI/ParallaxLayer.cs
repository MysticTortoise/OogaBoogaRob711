using UnityEngine;
using UnityEngine.UI;

public class ParallaxLayer : MonoBehaviour
{
    private Material material;
    private Transform cameraTransform;
    [SerializeField] private Vector2 scrollSpeed;

    private void Start()
    {
        material = GetComponent<Image>().material;
        cameraTransform = FindAnyObjectByType<Camera>().transform;
    }

    private void Update()
    {
        material.SetVector("_UVOffset", cameraTransform.position * scrollSpeed);
    }
}
