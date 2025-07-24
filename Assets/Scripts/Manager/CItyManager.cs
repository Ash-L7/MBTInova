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
            // TODO: Add building to cityData.buildings
        }
        else
        {
            Debug.Log("Not enough money to place building!");
        }
    }


    public void UpdateProduction()
    {
        // TODO: Building output resources
    }
}