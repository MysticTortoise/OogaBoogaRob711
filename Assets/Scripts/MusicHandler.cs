using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    private static MusicHandler instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null)
        {
            if(GetComponent<AudioSource>().clip != instance.GetComponent<AudioSource>().clip)
            {
                Destroy(instance.gameObject);
                
            } else
            {
                Destroy(this.gameObject);
                return;
            }
        }
        instance = this;
        transform.parent = null;
        DontDestroyOnLoad(this.gameObject);
    }

    public static void StopMusic()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
    }
}