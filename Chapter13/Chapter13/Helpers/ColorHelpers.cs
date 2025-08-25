
using Chapter13.Domain;
using Microsoft.Xna.Framework;

namespace Chapter13.Helpers;

public static class ColorHelpers
{
    public static Color GetFactionColor(this Faction owner) => owner switch
    {
        Faction.Neutral => Color.White,
        Faction.Military => Color.Blue,
        Faction.Criminal => Color.Red,
        Faction.Alien => Color.Pink,
        _ => Color.Gray
    };
}