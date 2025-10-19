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
        cooldownCompleteEffect.color = new Color(255, 255, 255, 175);
        cooldownCompleteEffect.rectTransform.localScale = Vector3.one;
        cooldownCompleteEffect.gameObject.SetActive(true);

        StartCoroutine(PlayCooldownEffect(cooldownTime));
    }

    IEnumerator PlayCooldownEffect(float cooldownTime)
    {
        DOTween.Sequence()
            .Append(cooldownImage.DOFillAmount(0, cooldownTime))
            .Join(cooldownImage.DOColor(new Color(255, 255, 255, 0), cooldownTime)
            .SetEase(Ease.Linear));
        yield return DOTween.Sequence().WaitForCompletion();
        DOTween.Sequence()
            .Append(cooldownCompleteEffect.DOFade(0, 0.3f))
            .Join(cooldownCompleteEffect.rectTransform.DOScale(1.5f, 0.3f))
            .SetEase(Ease.OutQuint);
        yield return DOTween.Sequence().WaitForCompletion();
        cooldownCompleteEffect.gameObject.SetActive(false);
    }

}
