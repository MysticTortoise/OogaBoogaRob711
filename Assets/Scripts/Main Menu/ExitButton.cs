using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class ExitButton : MonoBehaviour
{
    bool confirmExit = false;
    bool isAnimating = false;
    [SerializeField] float waitDuration; // how long to wait before returning to x button state
    [SerializeField] GameObject exitButtonBackground;
    [SerializeField] Animator backgroundAnimator;
    [SerializeField] Button confirmButton;
    [SerializeField] TextMeshProUGUI confirmText;

    [SerializeField] Image deltarune;
    [SerializeField] AudioSource deltaruneAudio;



    public void OnExitButtonPressed()
    {
        if (confirmExit == false && isAnimating == false)
        {
            exitButtonBackground.SetActive(true);
            StartCoroutine(ExpandAnimation());
        }
        else if (confirmExit == true && isAnimating == false)
        {
            Debug.Log("goodbye!");
            StartCoroutine(ExitGame());
        }
    }

    IEnumerator ExpandAnimation()
    {
        isAnimating = true;
        confirmButton.interactable = false;
        confirmText.text = "CONFIRM (3)";
        backgroundAnimator.SetTrigger("ExitButtonEnter");
        yield return new WaitForSeconds(backgroundAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        confirmText.text = "CONFIRM (2)";
        yield return new WaitForSeconds(1);
        confirmText.text = "CONFIRM (1)";
        yield return new WaitForSeconds(1);
        confirmText.text = "CONFIRM";
        confirmButton.interactable = true;

        isAnimating = false;
        confirmExit = true;

        yield return new WaitForSeconds(waitDuration);

        backgroundAnimator.SetTrigger("ExitButtonExit");
        confirmButton.interactable = false;
        yield return new WaitForSeconds(backgroundAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        isAnimating = false;
        confirmExit = false;
        exitButtonBackground.SetActive(false);
    }

    IEnumerator ExitGame()
    {
        StopCoroutine(ExpandAnimation());

        deltarune.gameObject.SetActive(true);
        deltaruneAudio.Play();
        yield return new WaitForSeconds(0.5f);

        #if UNITY_EDITOR
        Debug.Break();
        #endif

        Application.Quit();
    }
}