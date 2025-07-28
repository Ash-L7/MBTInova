using UnityEngine;

public class CityManager : MonoBehaviour
{
    private CityData cityData;

    public void Init(CityData data)
    {
        cityData = data;
    }

    public int buildingCost = 200;

    public void PlaceBuilding(string type, Vector2Int position)
    {
        if (ResourceManager.Instance.SpendMoney(buildingCost))
        {
            Debug.Log($"Placing building: {type} at {position}");

            BuildingData newBuilding = new BuildingData();
            newBuilding.buildingID = System.Guid.NewGuid().ToString();
            newBuilding.type = type;

            switch (type)
            {
                case "ScienceLab":
                    newBuilding.domain = BuildingDomain.Science;
                    newBuilding.baseMoneyProduction = 5;
                    newBuilding.baseScienceProduction = 20;
                    newBuilding.baseFoodProduction = 0;
                    newBuilding.baseEnergyProduction = 0;
                    break;
                case "Farm":
                    newBuilding.domain = BuildingDomain.Logistics;
                    newBuilding.baseMoneyProduction = 2;
                    newBuilding.baseScienceProduction = 0;
                    newBuilding.baseFoodProduction = 25;
                    newBuilding.baseEnergyProduction = 0;
                    break;
            }

            cityData.buildings.Add(newBuilding);
        }
        else
        {
            Debug.Log("Not enough money to place building!");
        }
    }



    public void UpdateProduction()
    {
        foreach (var building in cityData.buildings)
        {
            float moneyOutput = building.baseMoneyProduction * building.productionMultiplier;
            float scienceOutput = building.baseScienceProduction * building.productionMultiplier;
            float foodOutput = building.baseFoodProduction * building.productionMultiplier;
            float energyOutput = building.baseEnergyProduction * building.productionMultiplier;

            ResourceManager.Instance.AddMoney(Mathf.RoundToInt(moneyOutput));
            ResourceManager.Instance.AddScience(Mathf.RoundToInt(scienceOutput));
            ResourceManager.Instance.AddFood(Mathf.RoundToInt(foodOutput));
            ResourceManager.Instance.AddEnergy(Mathf.RoundToInt(energyOutput));

            Debug.Log($"[Produce] {building.buildingID} produced Money: {moneyOutput}, Science: {scienceOutput}, Food: {foodOutput}, Energy: {energyOutput}");
        }
    }

    void Start()
    {
        InvokeRepeating(nameof(UpdateProduction), 5f, 5f);
    }

}
