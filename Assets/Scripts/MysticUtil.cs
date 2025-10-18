using UnityEngine;


public class MysticUtil
{
    public static float Damp(float a, float b, float lambda, float dt)
    {
        return Mathf.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }

    public static Vector3 DampVector(Vector3 a, Vector3 b, float lambda, float dt)
    {
        return new Vector3(
            Damp(a.x, b.x, lambda, dt),
            Damp(a.y, b.y, lambda, dt),
            Damp(a.z, b.z, lambda, dt)
        );
    }
}
