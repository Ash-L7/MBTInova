using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class CityData
{
    public ResourceData resources = new ResourceData();
    public List<CitizenData> citizens = new List<CitizenData>();
    public List<BuildingData> buildings = new List<BuildingData>();
    public List<string> triggeredEvents = new List<string>();

    public int currentDay = 0;

    public CitizenData GetCitizenByID(string citizenID)
    {
        return citizens.Find(c => c.citizenID == citizenID);
    }

    public BuildingData GetBuildingByID(string buildingID)
    {
        return buildings.Find(b => b.buildingID == buildingID);
    }

    public void InitializeDefault()
    {
        resources = new ResourceData();
        citizens.Clear();
        buildings.Clear();
        triggeredEvents.Clear();
        currentDay = 0;
    }
}