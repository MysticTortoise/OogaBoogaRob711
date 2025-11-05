using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectIntroAnim : MonoBehaviour
{
    public static bool WasInLevel = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Image>().color = WasInLevel ? Color.white : Color.black;
        WasInLevel = false;
    }

    public void TransitionDone()
    {
        GetComponent<Image>().color = Color.black;
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
