using UnityEngine;


public enum HitType
{
    Stick = 100,
    Rock = 20,
    Dash = 0,
}
public class HittableBase : MonoBehaviour
{
    public virtual void Hit(HitType type)
    {
        
    }
}
