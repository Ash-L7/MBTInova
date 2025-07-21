using UnityEngine;

public class CityManager : MonoBehaviour
{
    private CityData cityData;

    public void Init(CityData data)
    {
        cityData = data;
    }

    public void Tick()
    {
       // for example
        cityData.resources.money += 10;
        cityData.resources.sciencePoints += 5;
        cityData.resources.food += 8;
        cityData.resources.energy += 6;

        Debug.Log($"[City Tick] Money:{cityData.resources.money} | Science:{cityData.resources.sciencePoints}");
    }
    public void PlaceBuilding(string type, Vector2Int position)
    {
        Debug.Log($"Placing building: {type} at {position}");
    }

    public void UpdateProduction()
    {
        // TODO: Building output resources
    }
}