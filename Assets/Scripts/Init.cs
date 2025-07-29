using UnityEngine;

public class Init : MonoBehaviour
{
    void Start()
    {
        var cityData = GameManager.Instance.cityData;

        DebugPanelController debugPanel = FindObjectOfType<DebugPanelController>();

        if (CitizenManager.Instance != null)
        {
            CitizenManager.Instance.SetDebugPanel(debugPanel);
            CitizenManager.Instance.Init(cityData);
        }

        CityManager cityManager = FindObjectOfType<CityManager>();
        if (cityManager != null)
            cityManager.Init(cityData);
    }
}