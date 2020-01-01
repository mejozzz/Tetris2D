using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vectorf
{
    public static Vector2 Round(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }
}
