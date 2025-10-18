using UnityEngine;
using UnityEngine.UI;

public class ParallaxLayer : MonoBehaviour
{
    private Material material;
    private Transform cameraTransform;
    [SerializeField] private Vector2 scrollSpeed;

    private void Start()
    {
        material = new Material(GetComponent<Image>().material);
        cameraTransform = FindAnyObjectByType<Camera>().transform;
        GetComponent<Image>().material = material;
    }

    private void Update()
    {
        material.SetVector("_UVScale", new Vector2((float)Screen.width / Screen.height / (16f / 9f), 1));
        material.SetVector("_UVOffset", cameraTransform.position * scrollSpeed);
    }
}
