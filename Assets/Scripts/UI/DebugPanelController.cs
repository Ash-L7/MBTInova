using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanelController : MonoBehaviour
{
    public Button recruitButton;
    public Button buildButton;
    public Button eventButton;
    public TextMeshProUGUI resourceDisplay;

    void Start()
    {
        recruitButton.onClick.AddListener(() =>
        {
            GameManager.Instance.citizenManager.CreateCitizen("INTP");
            UpdateResources();
        });

        buildButton.onClick.AddListener(() =>
        {
            GameManager.Instance.cityManager.PlaceBuilding("MindLab", new Vector2Int(1, 1));
            UpdateResources();
        });

        eventButton.onClick.AddListener(() =>
        {
            GameManager.Instance.eventManager.ForceTrigger("INTP_vs_ESFJ");
        });

        UpdateResources();
    }

    void UpdateResources()
    {
        var r = GameManager.Instance.cityData.resources;
        resourceDisplay.text = $"Money: {r.money}\nScience: {r.sciencePoints}\nFood: {r.food}\nEnergy: {r.energy}";
    }
}