using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DebugPanelController : MonoBehaviour
{
    [Header("Buttons")]
    public Button recruitButton;
    public Button buildButton;
    public Button eventButton;

    [Header("UI Display")]
    public TextMeshProUGUI resourceDisplay;
    public TextMeshProUGUI citizenListDisplay;

    void Start()
    {
        recruitButton.onClick.AddListener(OnRecruitButtonClicked);
        buildButton.onClick.AddListener(OnBuildButtonClicked);
        eventButton.onClick.AddListener(OnEventButtonClicked);

        UpdateResources();
    }

    void UpdateResources()
    {
        var r = GameManager.Instance.cityData.resources;
        resourceDisplay.text = $"Money: {r.money}\nScience: {r.sciencePoints}\nFood: {r.food}\nEnergy: {r.energy}";
    }

    public void UpdateCitizenList(string listText)
    {
        if (citizenListDisplay != null)
            citizenListDisplay.text = listText;
    }

    void OnRecruitButtonClicked()
    {
        GameManager.Instance.citizenManager.CreateCitizen();
        UpdateResources();
    }

    void OnBuildButtonClicked()
    {
        GameManager.Instance.cityManager.PlaceBuilding("MindLab", new Vector2Int(1, 1));
        UpdateResources();
    }

    void OnEventButtonClicked()
    {
        GameManager.Instance.eventManager.ForceTrigger("INTP_vs_ESFJ");
    }
}