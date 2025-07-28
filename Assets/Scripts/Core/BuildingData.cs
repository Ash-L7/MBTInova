using System.Collections.Generic;

[System.Serializable]
public class BuildingData
{
    public string buildingID;
    public string type;
    public BuildingDomain domain;

    public int baseMoneyProduction = 10;
    public int baseScienceProduction = 5;
    public int baseFoodProduction = 8;
    public int baseEnergyProduction = 3;

    public float productionMultiplier = 1.0f;

    public List<string> assignedCitizenIDs = new List<string>();
}

public enum BuildingDomain
{
    Science,
    Art,
    Logistics,
    Exploration
}
