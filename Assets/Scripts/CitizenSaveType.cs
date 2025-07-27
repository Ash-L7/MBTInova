using GameDefs;
using System;
using System.Collections.Generic;

[Serializable]
public class CitizenSaveData
{
    public string mbtiType;
    public string currentBuildingID;
    public int jobSatisfaction;
    public int highSatisfactionDays;
    public CitizenState currentState;
}

[Serializable]
public class CitizenDataWrapper
{
    public List<CitizenSaveData> citizens;
}
