using Chapter13.Domain;
using Microsoft.Xna.Framework;

namespace Chapter13.Components;

public class TextLabelComponent(string text)
{
    public string Text { get; set; } = text;
    public Color Color { get; set; } = Color.White;
    public bool Centered { get; set; }
    public GameFont GameFont { get; set; }
}