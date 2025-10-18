using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class PlayButton : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] TextMeshProUGUI playButtonText;

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

            //isClicked = false;
        }
    }

    IEnumerator PlayAnimationSequence()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(playButton.transform.DOScale(1f, 0.3f)).SetEase(Ease.OutExpo);
        sequence.Append(playButton.transform.DOScale(1.1f, 0.2f)).SetEase(Ease.OutExpo);

        yield return null;
    }
}
