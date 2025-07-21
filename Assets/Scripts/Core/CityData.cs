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
    public int temperamentNF = 100;

    public List<string> activeBuffs = new();
    public List<string> systemUnlocks = new();

    public int currentDay = 0;

    public void InitializeDefault()
    {
        resources = new ResourceData();
        citizens.Clear();
        buildings.Clear();
        triggeredEvents.Clear();
        activeBuffs.Clear();
        systemUnlocks.Clear();
        currentDay = 0;
    }
}