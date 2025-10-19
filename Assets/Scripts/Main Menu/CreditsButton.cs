using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreditsButton : MonoBehaviour
{
    [SerializeField] Image creditsBackground;
    [SerializeField] Button creditsOpenButton;
    [SerializeField] Button creditsCloseButton;
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
            if(button.name != "Credits Open" || button.name != "Credits Close")
                button.interactable = false;
        }

        if(buttonMode == 0)
        {
            StartCoroutine(OpenCredits());
        }
        else
        {
            StartCoroutine(CloseCredits());
        }


    }

    IEnumerator OpenCredits()
    {
        creditsOpenButton.interactable = false;
        creditsAnimation.SetTrigger("OpenCredits");

        yield return new WaitForSeconds(creditsAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        creditsCloseButton.interactable = true;
        buttonMode = 1;
        isAnimating = false;
    }

    IEnumerator CloseCredits()
    {
        creditsCloseButton.interactable = false;
        creditsAnimation.SetTrigger("CloseCredits");

        yield return new WaitForSeconds(creditsAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        creditsOpenButton.interactable = true;

        foreach (Button button in FindObjectsByType<Button>(FindObjectsSortMode.None))
        {
            if (button.name != "Credits Open" || button.name != "Credits Close")
                button.interactable = true;
        }
        buttonMode = 0;
        isAnimating = false;
    }

}
