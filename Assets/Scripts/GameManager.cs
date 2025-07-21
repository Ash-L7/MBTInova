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

    private int manualTickCount = 0;

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
        if (currentState != GameState.Playing)
            return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            ManualTick();
        }
    }

    public void ManualTick()
    {
        manualTickCount++;
        Debug.Log($"Manual Tick {manualTickCount}");

        cityManager.Tick();
        citizenManager.UpdateCitizens();
        cityData.triggeredEvents.Clear();

        if (manualTickCount % 5 == 0)
        {
            //eventManager.CheckTriggers();
        }
    }

    public void InitializeGame()
    {
        cityData = new CityData();
        cityData.InitializeDefault();

        cityManager.Init(cityData);
        citizenManager.Init(cityData);
        //eventManager.Init(cityData);
        saveManager.Init(cityData);

        Debug.Log("Game initialized.");
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
}

public enum GameState
{
    Playing,
    Paused,
    Menu
}