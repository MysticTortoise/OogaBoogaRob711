using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] Image hpFill; // the yellow part or something
    [SerializeField] Image hpChange; // the white part
    [SerializeField] float changeAnimationDuration;

    
    public void SetHealthText(string newText)
    {
        hpText.text = newText;
    }

    /// <summary>
    /// sets the health display to the desired value
    /// this will <b><u>NOT</u></b> play any animations. it only sets the display directly.
    /// </summary>
    /// <param name="newDisplayValue">the new overlay fillAmount value. <b>FINAL VALUE MUST BE BETWEEN 0 TO 1</b></param>
    public void SetHealthBar(float newDisplayValue)
    {
        DOTween.Kill(hpFill);
        DOTween.Kill(hpChange);
        hpFill.fillAmount = newDisplayValue;
        hpChange.fillAmount = newDisplayValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newDisplayValue">the new overlay fillAmount value. <b>FINAL VALUE MUST BE BETWEEN 0 TO 1</b></param>
    /// <param name="playAnimation">whether to play the health change animation</param>
    public void SetHealthBar(float newDisplayValue, bool playAnimation)
    {
        if (newDisplayValue == hpFill.fillAmount)
            return;

        if (newDisplayValue < hpFill.fillAmount) // damage
        {
            hpFill.fillAmount = newDisplayValue;
            if (playAnimation)
                hpChange.DOFillAmount(hpFill.fillAmount, changeAnimationDuration).SetEase(Ease.OutQuint);
            else
                hpChange.fillAmount = hpFill.fillAmount;
        }
        else if (newDisplayValue > hpFill.fillAmount) // healing
        {
            hpChange.fillAmount = newDisplayValue;
            if (playAnimation)
                hpFill.DOFillAmount(hpChange.fillAmount, changeAnimationDuration).SetEase(Ease.OutQuint);
            else
                hpFill.fillAmount = hpChange.fillAmount;
        }
    }

    /// <summary>
    /// changes the health display by the given value
    /// will play the health change animation unless you use the overload function
    /// </summary>
    /// <param name="changeValue">the value to change health fillAmount by. <b>FINAL VALUE MUST BE BETWEEN 0 TO 1</b></param>
    public void ChangeHealthBar(float changeValue)
    {
        if (changeValue == 0)
            return;

        DOTween.Kill(hpFill);
        DOTween.Kill(hpChange);

        if (changeValue < 0) // damage
        {
            hpFill.fillAmount += changeValue;

            hpChange.DOFillAmount(hpFill.fillAmount, changeAnimationDuration).SetEase(Ease.OutQuint);
        }
        else if (changeValue > 0) // healing
        {
            hpChange.fillAmount += changeValue;

            hpFill.DOFillAmount(hpChange.fillAmount, changeAnimationDuration).SetEase(Ease.OutQuint);
        }
    }

    /// <summary>
    /// changes the health display by the given value
    /// this will play the health change animation unless specified otherwise
    /// </summary>
    /// <param name="changeValue">the value to change health fillAmount by. <b>FINAL VALUE MUST BE BETWEEN 0 TO 1</b></param>
    /// <param name="playAnimation">whether or not to play the health change animation</param>
    public void ChangeHealthBar(float changeValue, bool playAnimation)
    {
        if(changeValue == 0)
            return;

        DOTween.Kill(hpFill);
        DOTween.Kill(hpChange);

        if (changeValue < 0) // damage
        {
            hpFill.fillAmount += changeValue;

            if(playAnimation)
                hpChange.DOFillAmount(hpFill.fillAmount, changeAnimationDuration).SetEase(Ease.OutQuint);
            else
                hpChange.fillAmount = hpFill.fillAmount;
        }
        else if (changeValue > 0) // healing
        {
            hpChange.fillAmount += changeValue;

            hpFill.DOFillAmount(hpChange.fillAmount, changeAnimationDuration).SetEase(Ease.OutQuint);

            if(playAnimation)
                hpFill.DOFillAmount(hpChange.fillAmount, changeAnimationDuration).SetEase(Ease.OutQuint);
            else
                hpFill.fillAmount = hpChange.fillAmount;
        }
    }

}
