using UnityEngine;
using System.Text;
using System.Collections.Generic;
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
            newCitizen.currentBuildingID = "DummyBuilding"; // just for testing
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

    private void RecalculateBuildingBuff(BuildingData building)
    {
        if (building == null) return;

        float baseMultiplier = 1.0f;

        foreach (string citizenID in building.assignedCitizenIDs)
        {
            CitizenData citizen = cityData.GetCitizenByID(citizenID);
            if (citizen == null) continue;

            if (IsMatch(citizen, building))
            {
                if (citizen.currentState == CitizenState.Promoted)
                {
                    baseMultiplier += 0.5f;
                    citizen.jobSatisfaction = Mathf.Min(citizen.jobSatisfaction + 25, 100);
                }
                else
                {
                    baseMultiplier += 0.2f;
                    citizen.jobSatisfaction = Mathf.Min(citizen.jobSatisfaction + 20, 100);
                }
            }
            else
            {
                citizen.jobSatisfaction = Mathf.Max(citizen.jobSatisfaction - 10, 0);
            }
        }

        Dictionary<string, int> temperamentCount = new Dictionary<string, int>();
        foreach (string citizenID in building.assignedCitizenIDs)
        {
            CitizenData citizen = cityData.GetCitizenByID(citizenID);
            if (citizen == null) continue;

            if (!temperamentCount.ContainsKey(citizen.temperament))
                temperamentCount[citizen.temperament] = 0;
            temperamentCount[citizen.temperament]++;
        }

        foreach (var kvp in temperamentCount)
        {
            if (kvp.Value >= 2)
            {
                float synergyBuff = 0.1f * (kvp.Value - 1);
                baseMultiplier += synergyBuff;

                foreach (string citizenID in building.assignedCitizenIDs)
                {
                    CitizenData citizen = cityData.GetCitizenByID(citizenID);
                    if (citizen != null && citizen.temperament == kvp.Key)
                    {
                        citizen.jobSatisfaction = Mathf.Min(citizen.jobSatisfaction + 5, 100);
                    }
                }
            }
        }

        building.productionMultiplier = Mathf.Min(baseMultiplier, 3.0f);

        Debug.Log($"[RecalculateBuff] {building.buildingID} new productionMultiplier: {building.productionMultiplier}");
    }

    public void AssignCitizenToBuilding(CitizenData citizen, BuildingData newBuilding)
    {
        if (!string.IsNullOrEmpty(citizen.currentBuildingID) && citizen.currentBuildingID != newBuilding.buildingID)
        {
            BuildingData oldBuilding = cityData.GetBuildingByID(citizen.currentBuildingID);
            if (oldBuilding != null)
            {
                oldBuilding.assignedCitizenIDs.Remove(citizen.citizenID);
                RecalculateBuildingBuff(oldBuilding);
                Debug.Log($"[Assign] Removed {citizen.citizenID} from {oldBuilding.buildingID}");
            }
        }

        citizen.currentBuildingID = newBuilding.buildingID;

        if (!newBuilding.assignedCitizenIDs.Contains(citizen.citizenID))
        {
            newBuilding.assignedCitizenIDs.Add(citizen.citizenID);
        }

        RecalculateBuildingBuff(newBuilding); // 重新计算新建筑buff
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
                Debug.Log("==> FSM: Add SatDay");
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
            citizen.jobSatisfaction = Mathf.Min(citizen.jobSatisfaction + 5, 100);
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
}
