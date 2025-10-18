using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] Image hpOverlay; // the yellow part or something
    [SerializeField] Image hpChangeIndicator; // the white part


    /// <summary>
    /// sets the health display to the desired value
    /// this will <b><u>NOT</b></u> play any animations. it only sets the display directly.
    /// </summary>
    /// <param name="newDisplayValue">the new overlay fillAmount value. <b>MUST BE BETWEEN 0 TO 1</b></param>
    public void SetHealthDisplay(float newDisplayValue)
    {
        hpOverlay.fillAmount = newDisplayValue;
        hpChangeIndicator.fillAmount = newDisplayValue;
    }

}
