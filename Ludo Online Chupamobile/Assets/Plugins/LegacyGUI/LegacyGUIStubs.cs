using UnityEngine;

// Lightweight compatibility stubs for removed legacy GUI types.
// These exist only to allow older plugins (like iTween) to compile in newer Unity versions.
// They are minimal and intentionally do not implement full legacy behavior.
namespace UnityEngine
{
    public class GUITexture : Behaviour
    {
        public Rect pixelInset;
        public Texture texture;
        public Color color;
        public Material material;
    }

    public class GUIText : Behaviour
    {
        public string text;
        public Material material;
        public Color color;
    }
}
