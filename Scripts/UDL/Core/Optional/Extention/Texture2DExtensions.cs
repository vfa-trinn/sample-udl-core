using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDL.Core
{
    public static class Texture2DExtensions
    {
        public static Sprite ConvertToSprite(this Texture2D texture)
        {
            if (texture == null) return null;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }

        public static Sprite ConvertToSprite(this Texture2D texture, Vector2 pivot)
        {
            if (texture == null) return null;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot);
        }
    }
}
