using UnityEngine;


public enum HitType
{
    Stick,
    Rock,
    Dash
}
public class HittableBase : MonoBehaviour
{
    public virtual void Hit(HitType type)
    {
        
    }
}
