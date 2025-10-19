using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// makes a single item display function
public class ItemDisplaySingle : MonoBehaviour
{
    [SerializeField] Image cooldownImage;
    [SerializeField] Image cooldownCompleteEffect;

    Coroutine cooldownRoutine;

    public void StartItemCooldown(float cooldownTime)
    {
        if (cooldownRoutine != null)
            StopCoroutine(cooldownRoutine);

        DOTween.Kill(cooldownImage.rectTransform);
        DOTween.Kill(cooldownImage.sprite);
        DOTween.Kill(cooldownImage);
        DOTween.Kill(cooldownCompleteEffect.rectTransform.localScale);
        DOTween.Kill(cooldownCompleteEffect.rectTransform);
        DOTween.Kill(cooldownCompleteEffect.sprite);
        DOTween.Kill(cooldownCompleteEffect);

        cooldownCompleteEffect.DOFade(0.7f, 0);
        cooldownImage.DOFade(0.7f, 0);
        cooldownCompleteEffect.rectTransform.localScale = new Vector3(1, 1);
        cooldownCompleteEffect.rectTransform.DOScale(new Vector3(1, 1), 0);
        cooldownCompleteEffect.gameObject.SetActive(false);
        cooldownImage.gameObject.SetActive(true);
        cooldownImage.DOFillAmount(1, 0);

        cooldownRoutine = StartCoroutine(PlayCooldownEffect(cooldownTime));
    }

    IEnumerator PlayCooldownEffect(float cooldownTime)
    {
        yield return DOTween.Sequence()
        .Append(cooldownImage.DOFillAmount(0, cooldownTime).SetEase(Ease.Linear))
        .Join(cooldownImage.DOFade(0.15f, cooldownTime).SetEase(Ease.Linear))
        .WaitForCompletion();

        cooldownImage.gameObject.SetActive(false);
        cooldownCompleteEffect.gameObject.SetActive(true);

        yield return DOTween.Sequence()
        .Append(cooldownCompleteEffect.DOFade(0, 0.2f).SetEase(Ease.Linear))
        .Join(cooldownCompleteEffect.rectTransform.DOScale(new Vector3(1.5f, 1.5f), 0.2f).SetEase(Ease.OutQuad))
        .WaitForCompletion();

        cooldownCompleteEffect.gameObject.SetActive(false);
    }

}