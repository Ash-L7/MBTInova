using System;
using System.Collections.Generic;
using UnityEngine;
using GameDefs;

[System.Serializable]
public class BuildingData
{
    public string buildingID;
    public string type;
    public Vector2Int position;
    public int level = 1;

    public List<string> assignedCitizenIDs = new List<string>();

    public BuildingDomain domain;
    public float productionMultiplier = 1.0f;

    public int baseOutput = 100;
    public bool hasSynergyBonus = false;
    public bool isUpgradable = true;
}