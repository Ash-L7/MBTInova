using UnityEngine;
using UnityEngine.UIElements;

public class BuildingCard : VisualElement
{
    public string BuildingName { get; private set; }
    
    public BuildingCard(string name)
    {
        BuildingName = name;
        
        AddToClassList("building-card");
        
        Add(new Label(name) {
            style = {
                fontSize = 18,
                unityFontStyleAndWeight = FontStyle.Bold,
                marginBottom = 10,
                color = new Color(0.2f, 0.2f, 0.2f)
            }
        });
        
        Add(new Label("Efficiency: 100%") {
            name = "efficiency-label",
            style = {
                fontSize = 14,
                color = new Color(0.3f, 0.3f, 0.3f)
            }
        });
    }
    
}