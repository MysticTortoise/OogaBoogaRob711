using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; 
using System.Collections;
using UnityEngine.SceneManagement;


public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] Button levelButton;
    [SerializeField] TextMeshProUGUI levelButtonText;
    [SerializeField] RectTransform textFinalPos;
    [SerializeField] RectTransform moveToCenterPos;
    [SerializeField] Image fadeToWhite;
    [SerializeField] AudioSource enterLevelSFX;
    [SerializeField] string levelToLoad;
    [SerializeField] private int levelNumber;

    bool lockActions = false; // prevent all buttons from being interacted with

    private void Start()
    {
        if (SaveData.levelsBeaten < levelNumber - 1)
        {
            levelButton.interactable = false;
        }
    }

    public void LockActions()
    {
        lockActions = true;
        //levelButton.interactable = false;
    }

    public void OnHover()
    {
        if(lockActions || !levelButton.interactable) 
            return;

        DOTween.Kill(levelButtonText);
        DOTween.Kill(levelButton);

        levelButtonText.rectTransform.localScale = new Vector2(1.5f, 1.5f);
        levelButtonText.DOFade(0f, 0f);

        levelButtonText.gameObject.SetActive(true);
        levelButtonText.rectTransform.DOMove(textFinalPos.position, 0.2f).SetEase(Ease.OutQuint);
        levelButtonText.rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutQuint);
        levelButtonText.DOFade(1f, 0.2f).SetEase(Ease.OutQuint);

        levelButton.transform.DOScale(1.15f, 0.2f).SetEase(Ease.OutQuint);
    }

    public void OnExit()
    {
        if(lockActions || !levelButton.interactable) 
            return;

        DOTween.Kill(levelButtonText);
        DOTween.Kill(levelButton);

        levelButtonText.rectTransform.DOMove(levelButton.image.rectTransform.position, 0.2f).SetEase(Ease.OutQuint);
        levelButtonText.rectTransform.DOScale(1.5f, 0.2f).SetEase(Ease.OutQuint);
        levelButtonText.DOFade(0f, 0.2f).SetEase(Ease.OutQuint);

        levelButton.transform.DOScale(1f, 0.2f).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            levelButtonText.gameObject.SetActive(false);
        });
    }

    public void OnClick()
    {
        if (lockActions || !levelButton.interactable) 
            return;

        // lock all buttons to prevent multiple clicks
        foreach (LevelSelectButton button in FindObjectsByType<LevelSelectButton>(FindObjectsSortMode.None))
        {
            button.LockActions();
        }

        StartCoroutine(StartTransitionSequence());
    }

    IEnumerator StartTransitionSequence()
    {
        fadeToWhite.gameObject.SetActive(true);

        // button expands and goes back
        DOTween.Sequence()
            .Append(levelButton.transform.DOScale(1.7f, 0.1f).SetEase(Ease.OutQuint))
            .Append(levelButton.transform.DOScale(1.3f, 0.9f).SetEase(Ease.OutQuint));

        // text pops out and fades away
        yield return DOTween.Sequence()
            .Append(levelButtonText.rectTransform.DOScale(1.5f, 0.3f).SetEase(Ease.OutQuint))
            .Join(levelButtonText.DOFade(0f, 0.3f).SetEase(Ease.OutQuint))
            .WaitForCompletion();

        // make button go to center and fade white
        enterLevelSFX.Play();
        yield return DOTween.Sequence()
            .Append(levelButton.transform.DOMove(moveToCenterPos.position, 4f).SetEase(Ease.OutQuad))
            .Join(fadeToWhite.DOFade(1f, 4f).SetEase(Ease.Linear))
            .WaitForCompletion();

        SceneManager.LoadScene(levelToLoad);
    }
}
