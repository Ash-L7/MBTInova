using UnityEngine;
using System.Collections.Generic;

public enum Temperament { NT, NF, SJ, SP }

public static class TemperamentData
{
    public static Dictionary<Temperament, (string name, Color mainColor, Color lightColor)> TypeInfo = 
        new Dictionary<Temperament, (string, Color, Color)>()
    {
        { 
            Temperament.NT, 
            ("Analysts", 
            new Color(0.47f, 0.37f, 0.94f), // #785EF0
            new Color(0.72f, 0.66f, 0.97f)) // #B7A9F7
        },
        { 
            Temperament.NF, 
            ("Diplomats",
            new Color(0.00f, 0.70f, 0.58f), // #00B294
            new Color(0.50f, 0.85f, 0.79f)) // #80D9CA
        },
        { 
            Temperament.SJ, 
            ("Sentinels", 
            new Color(0.39f, 0.56f, 1.00f), // #648FFF
            new Color(0.70f, 0.78f, 1.00f)) // #B2C7FF
        },
        { 
            Temperament.SP, 
            ("Explorers", 
            new Color(1.00f, 0.69f, 0.00f), // #FFB000
            new Color(1.00f, 0.84f, 0.50f)) // #FFD580
        }
    };
}