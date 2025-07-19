using UnityEngine;
using System.Text;
using GameDefs;

public class CitizenManager : MonoBehaviour
{
    private CityData cityData;

    [Header("UI")]
    public DebugPanelController debugPanel;

    private string[] mbtiPool = {
        "INTP","ENTP","INFJ","ENFJ","INFP","ENFP",
        "ISTJ","ESTJ","ISFJ","ESFJ","ISTP","ESTP",
        "ISFP","ESFP","INTJ","ENTJ"
    };

    public void Init(CityData data)
    {
        cityData = data;
    }

    public void CreateCitizen()
    {
        string mbtiType = mbtiPool[Random.Range(0, mbtiPool.Length)];
        CitizenData newCitizen = new CitizenData(mbtiType);
        cityData.citizens.Add(newCitizen);

        Debug.Log($"Created: {newCitizen.mbtiType} - {newCitizen.temperament} - {newCitizen.dominantFunction}/{newCitizen.auxiliaryFunction}");

        UpdateCitizensUI();
    }

    public BuildingDomain ResolveDomain(string temperament)
    {
        switch (temperament)
        {
            case "NT": return BuildingDomain.Science;
            case "NF": return BuildingDomain.Art;
            case "SJ": return BuildingDomain.Logistics;
            case "SP": return BuildingDomain.Exploration;
            default: return BuildingDomain.Logistics;
        }
    }

    public bool IsMatch(CitizenData citizen, BuildingData building)
    {
        var citizenDomain = ResolveDomain(citizen.temperament);
        return citizenDomain == building.domain;
    }

    public void ApplyBuff(CitizenData citizen, BuildingData building)
    {
        bool isSynergy = IsMatch(citizen, building);

        if (isSynergy)
        {
            if (citizen.currentState == CitizenState.Promoted)
            {
                citizen.jobSatisfaction += 25;
                building.productionMultiplier += 0.5f;
                Debug.Log($"Buff upgraded！{citizen.mbtiType} to {building.buildingID} +0.5");
            }
            else
            {
                citizen.jobSatisfaction += 20;
                building.productionMultiplier += 0.2f;
                Debug.Log($"Buff applied：{citizen.mbtiType} to {building.domain}");
            }
        }
        else
        {
            citizen.jobSatisfaction -= 10;
            Debug.Log($"not match：{citizen.mbtiType} not applicable {building.domain}");
        }
    }

    public void AssignCitizenToBuilding(CitizenData citizen, BuildingData building)
    {
        citizen.currentBuildingID = building.buildingID;

        if (!building.assignedCitizenIDs.Contains(citizen.citizenID))
        {
            building.assignedCitizenIDs.Add(citizen.citizenID);
        }

        ApplyBuff(citizen, building);
        UpdateCitizensUI();
    }
    
    public void UpdateCitizensUI()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var c in cityData.citizens)
        {
            sb.AppendLine($"{c.mbtiType} - {c.temperament} - {c.dominantFunction}/{c.auxiliaryFunction} - Satisfaction: {c.jobSatisfaction}");
        }
     
        if (debugPanel != null)
        {
            debugPanel.UpdateCitizenList(sb.ToString());
        }
    }

    public void UpdateCitizenState(CitizenData citizen)
    {
        switch (citizen.currentState)
        {
            case CitizenState.Idle:
                if (!string.IsNullOrEmpty(citizen.currentBuildingID))
                {
                    citizen.currentState = CitizenState.Working;
                    Debug.Log($"{citizen.mbtiType} start working");
                }
                break;

            case CitizenState.Working:
                if (citizen.jobSatisfaction >= 80)
                    citizen.highSatisfactionDays++;
                else
                    citizen.highSatisfactionDays = 0;

                if (citizen.highSatisfactionDays >= 3)
                {
                    citizen.currentState = CitizenState.Promoted;
                    citizen.isPromoted = true;
                    citizen.titleName = "Architect of Synergy";

                    BuildingData building = cityData.buildings.Find(b => b.buildingID == citizen.currentBuildingID);
                    if (building != null)
                    {
                        building.productionMultiplier += 0.3f;
                        Debug.Log($"{citizen.mbtiType} upgraded！output：{building.buildingID} +0.3");
                    }
                }
                break;

            case CitizenState.Promoted:
                citizen.jobSatisfaction += 5;

                if (citizen.functionStats.ContainsKey(citizen.dominantFunction))
                {
                    citizen.functionStats[citizen.dominantFunction] += 1;

                    if (citizen.functionStats[citizen.dominantFunction] > 100)
                        citizen.functionStats[citizen.dominantFunction] = 100;
                }

                break;
        }
    }

    public void UpdateCitizens()
    {
        foreach (var citizen in cityData.citizens)
        {
            UpdateCitizenState(citizen);
        }

        UpdateCitizensUI();
    }
}