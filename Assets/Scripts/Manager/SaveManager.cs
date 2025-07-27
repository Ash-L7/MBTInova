using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private CityData cityData;

    public void Init(CityData data)
    {
        cityData = data;
    }

    public void Save()
    {
        Debug.Log("Saving game...");
    }

    public void Load()
    {
        Debug.Log("Loading game...");
    }
}