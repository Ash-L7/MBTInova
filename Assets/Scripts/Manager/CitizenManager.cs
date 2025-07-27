using GameDefs;
using System.Resources;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using static CitizenSaveData;


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

    public int recruitCost = 100;

    public void Init(CityData data)
    {
        cityData = data;
    }

    public void CreateCitizen()
    {
        if (ResourceManager.Instance.SpendMoney(recruitCost))
        {
            string mbtiType = mbtiPool[Random.Range(0, mbtiPool.Length)];
            CitizenData newCitizen = new CitizenData(mbtiType);
            newCitizen.currentBuildingID = "DummyBuilding"; // just for testinggggg
            cityData.citizens.Add(newCitizen);

            Debug.Log($"Created: {newCitizen.mbtiType} - {newCitizen.temperament} - {newCitizen.dominantFunction}/{newCitizen.auxiliaryFunction}");

            UpdateCitizensUI();
        }
        else
        {
            Debug.Log("[CitizenManager] Not enough money to recruit a citizen!");
        }
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
            sb.AppendLine($"{c.mbtiType} - {c.currentState} - Satisfaction: {c.jobSatisfaction} - HighSatDays: {c.highSatisfactionDays}");
        }

        if (debugPanel != null)
        {
            debugPanel.UpdateCitizenList(sb.ToString());
        }
    }

    public void UpdateCitizenState(CitizenData citizen)
    {
        Debug.Log($"[FSM] State={citizen.currentState} SatDays={citizen.highSatisfactionDays}");

        if (citizen.currentState == CitizenState.Idle)
        {
            if (!string.IsNullOrEmpty(citizen.currentBuildingID))
            {
                citizen.currentState = CitizenState.Working;
                Debug.Log("==> FSM: Switch to Working");
            }
            return;
        }

        if (citizen.currentState == CitizenState.Working)
        {
            if (citizen.jobSatisfaction >= 80)
            {
                citizen.highSatisfactionDays++;
                Debug.Log("==>FSM: Add SatDay");
            }
            else
            {
                citizen.highSatisfactionDays = 0;
                Debug.Log("==> FSM: Reset SatDay");
            }

            if (citizen.highSatisfactionDays >= 3)
            {
                citizen.currentState = CitizenState.Promoted;
                Debug.Log("==> FSM: PROMOTED!");
            }

            return;
        }

        if (citizen.currentState == CitizenState.Promoted)
        {
            Debug.Log("==> FSM: Promoted tick +5 sat");
            citizen.jobSatisfaction += 5;
            return;
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




    

    public List<CitizenSaveData> GetAllCitizenData()
    {
        List<CitizenSaveData> data = new List<CitizenSaveData>();
        foreach (var c in cityData.citizens)
        {
            data.Add(new CitizenSaveData
            {
                mbtiType = c.mbtiType,
                currentBuildingID = c.currentBuildingID,
                jobSatisfaction = c.jobSatisfaction,
                highSatisfactionDays = c.highSatisfactionDays,
                currentState = c.currentState
            });
        }
        return data;
    }

    public void RestoreCitizensFromData(List<CitizenSaveData> data)
    {
        cityData.citizens.Clear();

        foreach (var d in data)
        {
            CitizenData c = new CitizenData(d.mbtiType)
            {
                currentBuildingID = d.currentBuildingID,
                jobSatisfaction = d.jobSatisfaction,
                highSatisfactionDays = d.highSatisfactionDays,
                currentState = d.currentState
            };

            cityData.citizens.Add(c);
        }

        UpdateCitizensUI();
    }

}