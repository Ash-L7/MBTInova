using UnityEngine;

public class CityManager : MonoBehaviour
{
    private CityData cityData;

    public void Init(CityData data)
    {
        cityData = data;
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