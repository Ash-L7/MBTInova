using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CitizenData
{
    public string citizenID;
    public string mbtiType;
    public string temperament;
    public string currentBuildingID;
    public int jobSatisfaction;
    public int[] functionStats = new int[4]; // Ne, Ni, Te, Ti for example

    public bool isPromoted;
}