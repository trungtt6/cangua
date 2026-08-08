using UnityEngine;
using UnityEngine.UI;

// Minimal compatibility layer for legacy GUITexture / GUIText for older plugins (iTween etc.).
// These classes forward common properties to uGUI components when present.

public class GUITexture : MonoBehaviour
{
    // Backing fields for cases where Image is not present
    [SerializeField]
    private Texture _texture;
    [SerializeField]
    private Color _color = Color.white;

    public Texture texture
    {
        get
        {
            var img = GetComponent<Image>();
            if (img != null && img.sprite != null) return img.sprite.texture;
            return _texture;
        }
        set
        {
            var img = GetComponent<Image>();
            if (img != null)
            {
                if (value == null) img.sprite = null;
                else img.sprite = Sprite.Create((Texture2D)value, new Rect(0,0,((Texture2D)value).width, ((Texture2D)value).height), new Vector2(0.5f,0.5f));
            }
            _texture = value;
        }
    }

    public Color color
    {
        get
        {
            var img = GetComponent<Image>();
            if (img != null) return img.color;
            return _color;
        }
        set
        {
            var img = GetComponent<Image>();
            if (img != null) img.color = value;
            _color = value;
        }
    }

    public Material material
    {
        get
        {
            var img = GetComponent<Image>();
            if (img != null) return img.material;
            return null;
        }
        set
        {
            var img = GetComponent<Image>();
            if (img != null) img.material = value;
        }
    }
}

public class GUIText : MonoBehaviour
{
    [SerializeField]
    private string _text = "";

    public string text
    {
        get
        {
            var t = GetComponent<Text>();
            if (t != null) return t.text;
            return _text;
        }
        set
        {
            var t = GetComponent<Text>();
            if (t != null) t.text = value;
            _text = value;
        }
    }

    public Material material
    {
        get
        {
            var t = GetComponent<Text>();
            if (t != null) return t.material;
            return null;
        }
        set
        {
            var t = GetComponent<Text>();
            if (t != null) t.material = value;
        }
    }
}
