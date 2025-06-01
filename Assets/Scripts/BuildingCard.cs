using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingCard : VisualElement
{
    public string BuildingName { get; private set; }

    public BuildingCard(string name, Action onClick = null)
    {
        BuildingName = name;

        AddToClassList("building-card");
        style.width = 150;
        style.height = 100;
        style.marginRight = 10;
        style.paddingTop = 10;
        style.paddingLeft = 10;
        style.backgroundColor = new Color(0.85f, 0.85f, 0.9f);
        style.borderTopLeftRadius = 8;
        style.borderTopRightRadius = 8;
        style.flexDirection = FlexDirection.Column;
        style.justifyContent = Justify.Center;

        Add(new Label(name)
        {
            style =
            {
                fontSize = 18,
                unityFontStyleAndWeight = FontStyle.Bold,
                marginBottom = 10,
                color = new Color(0.2f, 0.2f, 0.2f)
            }
        });

        Add(new Label("Efficiency: 130%")
        {
            name = "efficiency-label",
            style =
            {
                fontSize = 14,
                color = new Color(0.3f, 0.3f, 0.3f)
            }
        });

        if (onClick != null)
        {
            RegisterCallback<ClickEvent>(_ => onClick());
        }
    }
}