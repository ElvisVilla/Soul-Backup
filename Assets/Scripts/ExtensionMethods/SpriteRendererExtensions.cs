using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bissash;

public static class SpriteRendererExtensions
{
    public static void OrientationSprite(this SpriteRenderer renderer, float speed, ref FacingSide facingSide)
    {
        if (speed > 0f)
        {
            facingSide = FacingSide.Right;
            renderer.flipX = false;
        }
        else if (speed < 0f)
        {
            facingSide = FacingSide.Left;
            renderer.flipX = true;
        }
        else
        {
            if (facingSide == FacingSide.Left)
            {
                renderer.flipX = true;
            }
            else if (facingSide == FacingSide.Right)
            {
                renderer.flipX = false;
            }
        }
    }
}

public enum FacingSide
{
    Right = 0,
    Left = 1,
}
