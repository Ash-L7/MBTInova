using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public CityData cityData;

    public CityManager cityManager;
    public CitizenManager citizenManager;
    public EventManager eventManager;
    public SaveManager saveManager;

    public GameState currentState = GameState.Playing;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            
        }
    }

    public void InitializeGame()
    {
        cityData = new CityData();
        cityData.InitializeDefault();

        cityManager.Init(cityData);
        citizenManager.Init(cityData);
        eventManager.Init(cityData);
        saveManager.Init(cityData);

        Debug.Log("Game initialized.");
    }

    // Debug UI
    public void LoadTestScenario()
    {
        citizenManager.CreateCitizen();
        cityManager.PlaceBuilding("MindLab", new Vector2Int(1, 1));
        eventManager.ForceTrigger("INTP_vs_ESFJ");
    }

    public void SpendResource(string type, int amount)
    {
        switch (type)
        {
            case "money":
                cityData.resources.money -= amount;
                break;
            case "science":
                cityData.resources.sciencePoints -= amount;
                break;
            case "food":
                cityData.resources.food -= amount;
                break;
            case "energy":
                cityData.resources.energy -= amount;
                break;
        }
    }

    public void NextDayTick()
    {
        Debug.Log("=== NextDay Start ===");

        citizenManager.UpdateCitizens();    // update fsm
        cityManager.UpdateProduction();     // update production
        //resourceManager.ApplyDailyChanges();
        eventManager.CheckTriggers();

        Debug.Log("=== NextDay End ===");
    }

}

public enum GameState
{
    Playing,
    Paused,
    Menu
}