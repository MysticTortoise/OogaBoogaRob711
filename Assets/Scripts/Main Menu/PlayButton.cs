using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class PlayButton : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] TextMeshProUGUI playButtonText;
    [SerializeField] Animator playButtonAnimator;
    [SerializeField] Animator sceneTransitionAnimator;

    bool isClicked = false;
    
    public void OnPointerEnter()
    {
        if(isClicked) 
            return;

        playButton.transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit()
    {
        if(isClicked) 
            return;

        playButton.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnClick()
    {
        if (isClicked == false)
        {
            isClicked = true;

            StartCoroutine(PlayAnimationSequence());
        }
    }

    IEnumerator PlayAnimationSequence()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(playButton.transform.DOScale(1.5f, 0.8f)).SetEase(Ease.OutExpo);
        sequence.Join(playButton.image.rectTransform.DORotate(new Vector3(0, 0, 360), 0.8f, RotateMode.FastBeyond360).SetEase(Ease.OutQuart));
        sequence.Append(playButton.transform.DOScale(1, 0.2f)).SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(0.2f);
        playButtonAnimator.SetTrigger("PlayButtonCommence");

        yield return new WaitForSeconds(.2f);
        sceneTransitionAnimator.SetTrigger("StartTransition");

        yield return new WaitForSeconds(sceneTransitionAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level Select");
    }

}
