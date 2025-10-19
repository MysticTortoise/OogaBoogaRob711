using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private Image image;
    private float val = 1;
    private float speed;
    private Color startCol;
    private Color goalCol;
    private string toSceneName = "";

    [SerializeField] private Color initialFade;
    [SerializeField] private bool initialTo;
    [SerializeField] private float initialTime;
    [SerializeField] private bool doInitial;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        startCol = image.color;
        goalCol = image.color;

        if (doInitial)
        {
            DoFade(initialFade, initialTo, initialTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        val = Mathf.Clamp(val + Time.deltaTime / speed, 0, 1);
        image.color = Color.Lerp(startCol, goalCol, val);

        if (val >= 1 && toSceneName != "")
        {
            SceneManager.LoadScene(toSceneName);
        }
    }
    
    public void DoFade(Color color, bool fadeTo, float time, [CanBeNull] string sceneName = "")
    {
        goalCol = color;
        startCol = color;
        val = 0;
        speed = time;
        toSceneName = sceneName;
        if (fadeTo)
        {
            startCol.a = 0;
            goalCol.a = 1;
        }
        else
        {
            startCol.a = 1;
            goalCol.a = 0;
        }
    }
}
