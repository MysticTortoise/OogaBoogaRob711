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
    [SerializeField] string levelToLoad;

    bool lockActions = false; // prevent all buttons from being interacted with

    public void LockActions()
    {
        lockActions = true;
    }

    public void OnHover()
    {
        if(lockActions) 
            return;

        DOTween.Kill(levelButtonText);
        DOTween.Kill(levelButton);
        
        levelButton.transform.DOScale(1.15f, 0.2f).SetEase(Ease.OutQuint);
    }

    public void OnExit()
    {
        if(lockActions) 
            return;

        DOTween.Kill(levelButtonText);
        DOTween.Kill(levelButton);

        levelButton.transform.DOScale(1f, 0.2f).SetEase(Ease.OutQuint);
    }

    public void OnClick()
    {
        if (lockActions) 
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
        yield return null;
    }
}
