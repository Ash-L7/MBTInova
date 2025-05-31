using UnityEngine;
using UnityEngine.UIElements;

public class CitizenCard : VisualElement
{
    public Temperament Type { get; private set; }
    
    public CitizenCard(Temperament temp)
    {
        Type = temp;
        var typeData = TemperamentData.TypeInfo[temp];
        
        AddToClassList("citizen-card");
        style.backgroundColor = typeData.lightColor;
        
        Add(new Label(typeData.name) {
            style = {
                color = Color.white,
                fontSize = 14,
                unityFontStyleAndWeight = FontStyle.Bold,
                unityTextAlign = TextAnchor.MiddleCenter,
                marginTop = 10,
                textShadow = new StyleTextShadow() {
                    value = new TextShadow() {
                        offset = new Vector2(1, 1),
                        blurRadius = 2,
                        color = Color.black
                    }
                }
            }
        });
        
        Add(new VisualElement {
            style = {
                height = 5,
                width = Length.Percent(100),
                backgroundColor = typeData.mainColor,
                marginTop = 5
            }
        });
        
        Add(new Label(GetRandomStats()) {
            style = {
                fontSize = 12,
                color = new Color(0.3f, 0.3f, 0.3f),
                marginTop = 8,
                unityTextAlign = TextAnchor.MiddleCenter
            }
        });
    }
    
    string GetRandomStats()
    {
        return Type switch {
            Temperament.NT => "INT:12\nTECH:14",
            Temperament.NF => "EMP:15\nCREA:13",
            Temperament.SJ => "ORG:14\nDISC:16",
            Temperament.SP => "ADAP:15\nDEX:13",
            _ => "N/A"
        };
    }
}