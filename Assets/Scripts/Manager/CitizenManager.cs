using UnityEngine;

public class CitizenManager : MonoBehaviour
{
    private CityData cityData;

    public void Init(CityData data)
    {
        cityData = data;
    }

    public void CreateCitizen(string mbtiType)
    {
        Debug.Log($"Creating citizen of type: {mbtiType}");
    }

    public void UpdateCitizens()
    {
        //update FSM
    }
}