using UnityEngine;
using UnityEngine.UI;

public class CreditsButton : MonoBehaviour
{
    [SerializeField] Image creditsBackground;
    [SerializeField] Animator creditsAnimation;

    bool isAnimating = false;
    int buttonMode = 0; // 0 = open, 1 = close

    public void OnClick()
    {
        if(isAnimating)
            return;

        isAnimating = true;

        foreach(Button button in FindObjectsByType<Button>(FindObjectsSortMode.None))
        {
            if(button.name != "Credits Button")
                button.interactable = false;
        }

        if(buttonMode == 0)
        {
            creditsAnimation.SetTrigger("OpenCredits");
            buttonMode = 1;
        }
        else
        {
            creditsAnimation.SetTrigger("CloseCredits");
            buttonMode = 0;
        }


    }

}
