using UnityEngine;
using UnityEngine.UI;

public class PlayerBGUIHandler : MonoBehaviour
{
    private Material cloudMaterial;
    private void Start()
    {
        cloudMaterial = transform.Find("Clouds").GetComponent<Image>().material;
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(transform.parent.position.x);
        cloudMaterial.SetVector("UVOffset", new Vector2(transform.parent.position.x, 0));
    }
}
