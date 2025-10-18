using Unity.VisualScripting;
using UnityEngine;

public class Break_Glass : MonoBehaviour
{
    private SpriteRenderer renderer;
    private GameObject glassBreak;
    public Sprite brokenDoor;
    void Start()
    {
        glassBreak = transform.GetChild(0).gameObject;
        renderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Rock")
        {
            renderer.sprite = brokenDoor;
            glassBreak.SetActive(true);
        }
    }
    
}
