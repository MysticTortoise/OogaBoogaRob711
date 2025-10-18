using UnityEngine;

public class BreakableGlass : HittableBase
{
    [SerializeField] private Sprite brokenSprite;
    private bool broken = false;
    public override void Hit(HitType type)
    {
        if(broken) { return;}

        broken = true;
        GetComponent<SpriteRenderer>().sprite = brokenSprite;
        GetComponent<Collider2D>().enabled = false;
    }
}
