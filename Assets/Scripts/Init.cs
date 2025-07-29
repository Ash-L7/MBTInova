using UnityEngine;

public class Scene2Initializer : MonoBehaviour
{
    void Start()
    {
        var cityData = GameManager.Instance.cityData;
        DebugPanelController debugPanel = FindObjectOfType<DebugPanelController>();
        CitizenManager.Instance.SetDebugPanel(debugPanel);


        if (CitizenManager.Instance != null)
            CitizenManager.Instance.Init(cityData);
    }
}