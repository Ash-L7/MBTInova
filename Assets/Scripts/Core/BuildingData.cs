using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingData
{
    public string buildingID;
    public string type;
    public Vector2Int position;
    public int level;
    public List<string> assignedCitizenIDs = new List<string>();
}