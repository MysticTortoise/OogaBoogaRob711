using UnityEngine;


// makes multiple item displays function... more like a centralized controller script
public class ItemDisplay : MonoBehaviour
{
    [SerializeField] ItemDisplaySingle[] itemDisplays;

    public enum itemIDS
    {
        STICK = 0,
        ROCK = 1,
        DODGE = 2
    }

    /// <summary>
    /// DO NOT USE. for testing purposes only.
    /// </summary>
    /// <param name="itemID"></param>
    public void Test(int itemID)
    {
        StartItemCooldown(itemID, 2);
    }

    public void StartItemCooldown(int itemIndex, float cooldownTime)
    {
        if (itemIndex < 0 || itemIndex >= itemDisplays.Length)
        {
            Debug.LogError("ItemDisplay: StartItemCooldown: itemIndex out of range");
            return;
        }

        itemDisplays[itemIndex].StartItemCooldown(cooldownTime);
    }

}
