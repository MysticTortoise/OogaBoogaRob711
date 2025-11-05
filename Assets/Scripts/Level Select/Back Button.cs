using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class BackButton : MonoBehaviour
{
    [SerializeField] string targetScene;
    [SerializeField] Image buttonSprite;
    [SerializeField] Animator sceneTransition;

    bool lockActions = false;

    public void OnClick()
    {
        if(lockActions)
            return;

        StartCoroutine(BackButtonSequence());
    }

    public void OnPointerEnter()
    {
        if(lockActions)
            return;

        DOTween.Kill(buttonSprite);

        buttonSprite.transform.DOScale(1.15f, 0.2f).SetEase(Ease.OutQuint);
    }

    public void OnPointerExit()
    {
        if(lockActions)
            return;

        DOTween.Kill(buttonSprite);

        buttonSprite.transform.DOScale(1f, 0.2f).SetEase(Ease.OutQuint);
    }

    IEnumerator BackButtonSequence()
    {
        lockActions = true;
        
        // do a pop effect, then move off screen

        yield return DOTween.Sequence()
            .Append(buttonSprite.transform.DOScale(1.6f, 0.1f).SetEase(Ease.OutQuint))
            .Append(buttonSprite.transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuint))
            .Join(buttonSprite.transform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutQuint))
            .WaitForCompletion();

        // play scene transition animation
        sceneTransition.SetTrigger("TriggerReturnTransition");
    }
}
