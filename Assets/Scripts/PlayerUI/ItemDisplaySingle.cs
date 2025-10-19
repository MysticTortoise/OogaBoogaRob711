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
        StartCoroutine(PlayCooldownEffect(cooldownTime));
    }

    IEnumerator PlayCooldownEffect(float cooldownTime)
    {
        
    }

}
