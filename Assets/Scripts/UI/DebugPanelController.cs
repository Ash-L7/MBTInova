﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DebugPanelController : MonoBehaviour
{
    [Header("Buttons")]
    public Button recruitButton;
    public Button buildButton;
    public Button eventButton;
    public Button nextDayButton;

    [Header("UI Display")]
    public TextMeshProUGUI resourceDisplay;
    public TextMeshProUGUI citizenListDisplay;

    void Start()
    {
        recruitButton.onClick.AddListener(OnRecruitButtonClicked);
        buildButton.onClick.AddListener(OnBuildButtonClicked);
        eventButton.onClick.AddListener(OnEventButtonClicked);
        nextDayButton.onClick.AddListener(OnNextDayClicked);

        UpdateResources();
    }

    public void UpdateResources()
    {
        var r = ResourceManager.Instance.resourceData;
        resourceDisplay.text = $"Money: {r.money}\nScience: {r.sciencePoints}\nFood: {r.food}\nEnergy: {r.energy}";
    }

    public void UpdateCitizenList(string listText)
    {
        if (citizenListDisplay != null)
            citizenListDisplay.text = listText;

        UpdateResources();
    }

    void OnRecruitButtonClicked()
    {
        Debug.Log(">>> Recruit clicked");
        GameManager.Instance.citizenManager.CreateCitizen();
        UpdateResources();
        CitizenManager.Instance.UpdateCitizensUI();
    }

    void OnBuildButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    void OnEventButtonClicked()
    {
        GameManager.Instance.eventManager.ForceTrigger("INTP_vs_ESFJ");
    }


    void OnNextDayClicked()
    {
        Debug.Log(">>> NextDay clicked");
        GameManager.Instance.NextDayTick();
        UpdateResources();
    }


}
