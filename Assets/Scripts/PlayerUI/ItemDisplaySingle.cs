using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// makes a single item display function
public class ItemDisplaySingle : MonoBehaviour
{
    [SerializeField] Image cooldownImage;
    [SerializeField] Image cooldownCompleteEffect;


    public void StartItemCooldown(float cooldownTime)
    {
        DOTween.Kill(cooldownImage);
        DOTween.Kill(cooldownCompleteEffect);
        StopCoroutine(PlayCooldownEffect(cooldownTime));

        cooldownCompleteEffect.DOFade(0.7f, 0);
        cooldownImage.DOFade(0.7f, 0);
        cooldownCompleteEffect.rectTransform.localScale = Vector3.one;
        cooldownCompleteEffect.gameObject.SetActive(false);
        cooldownImage.gameObject.SetActive(true);
        cooldownImage.fillAmount = 1;

        StartCoroutine(PlayCooldownEffect(cooldownTime));
    }

    IEnumerator PlayCooldownEffect(float cooldownTime)
    {
        //var sequence1 = DOTween.Sequence();
        //sequence1.Append(cooldownImage.DOFillAmount(0, cooldownTime).SetEase(Ease.Linear));
        //sequence1.Join(cooldownImage.DOFade(0.15f, cooldownTime).SetEase(Ease.Linear));
        //yield return sequence1.WaitForCompletion();

        yield return DOTween.Sequence()
        .Append(cooldownImage.DOFillAmount(0, cooldownTime).SetEase(Ease.Linear))
        .Join(cooldownImage.DOFade(0.15f, cooldownTime).SetEase(Ease.Linear))
        .WaitForCompletion();

        cooldownImage.gameObject.SetActive(false);
        cooldownCompleteEffect.gameObject.SetActive(true);

        var sequence2 = DOTween.Sequence();
        sequence2.Append(cooldownCompleteEffect.DOFade(0, 0.2f).SetEase(Ease.Linear));
        sequence2.Join(cooldownCompleteEffect.rectTransform.DOScale(1.5f, 0.2f).SetEase(Ease.OutQuad));
        yield return sequence2.WaitForCompletion();

        cooldownCompleteEffect.gameObject.SetActive(false);
    }

}
