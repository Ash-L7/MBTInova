using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class CloudSave : MonoBehaviour
{
    public GridPlacement gridPlacement;
    private const string CLOUD_SAVE_BUILDINGS_KEY = "placed_buildings";

    [Header("Building Settings")]
    public string buildingsFolderPath = "Assets/Sprites/Houses/";

    [System.Serializable]
    public class BuildingData
    {
        public string prefabName;
        public Vector3 position;
        public Quaternion rotation;
    }

    [System.Serializable]
    public class BuildingDataWrapper
    {
        public List<BuildingData> buildings;
    }

    [System.Serializable]
    public class ResourceDataWrapper
    {
        public int money;
        public int sciencePoints;
        public int food;
        public int energy;
    }


    private Dictionary<string, GameObject> buildingPrefabs = new Dictionary<string, GameObject>();

    void Start()
    {
        InitializeServices();
        LoadAllBuildingPrefabs();
    }

    void LoadAllBuildingPrefabs()
    {
        buildingPrefabs.Clear();

#if UNITY_EDITOR
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { buildingsFolderPath });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null)
            {
                string cleanName = prefab.name; 
                buildingPrefabs[cleanName] = prefab;
                Debug.Log($"Loaded building prefab: {cleanName}");
            }
        }
#endif
    }

    async void InitializeServices()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
            await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
            Debug.LogWarning("User not signed in. Cloud Save requires authentication.");
    }

    public void OnSaveButtonPressed() => SaveAllGameData();
    public void OnLoadButtonPressed() => LoadAllGameData();

    async void SaveAllGameData()
    {
        var buildings = GameObject.FindGameObjectsWithTag("Building");
        var buildingsData = new BuildingDataWrapper { buildings = new List<BuildingData>() };

        foreach (var building in buildings)
        {
            string prefabName = GetOriginalPrefabName(building);
            if (!string.IsNullOrEmpty(prefabName) && buildingPrefabs.ContainsKey(prefabName))
            {
                buildingsData.buildings.Add(new BuildingData
                {
                    prefabName = prefabName,
                    position = building.transform.position,
                    rotation = building.transform.rotation
                });
            }
        }

        var savePayload = new Dictionary<string, object>
    {
        { CLOUD_SAVE_BUILDINGS_KEY, JsonUtility.ToJson(buildingsData) }
    };

        try
        {
            await CloudSaveService.Instance.Data.ForceSaveAsync(savePayload);
            Debug.Log("Building data saved.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }


    string GetOriginalPrefabName(GameObject instance)
    {
        var id = instance.GetComponent<BuildingIdentifier>();
        return id != null ? id.prefabName : null;
    }

    async void LoadAllGameData()
    {
        var keys = new HashSet<string>
    {
        CLOUD_SAVE_BUILDINGS_KEY
    };

        try
        {
            var data = await CloudSaveService.Instance.Data.LoadAsync(keys);

            if (data.TryGetValue(CLOUD_SAVE_BUILDINGS_KEY, out var bData))
            {
                ClearExistingBuildings();
                var wrapper = JsonUtility.FromJson<BuildingDataWrapper>(bData.ToString());
                foreach (var buildingData in wrapper.buildings)
                {
                    if (buildingPrefabs.TryGetValue(buildingData.prefabName, out GameObject prefab))
                    {
                        var newBuilding = Instantiate(prefab, buildingData.position, buildingData.rotation);
                        newBuilding.tag = "Building";
                        var id = newBuilding.AddComponent<BuildingIdentifier>();
                        id.prefabName = buildingData.prefabName;
                    }
                }
            }

            Debug.Log("Building data loaded.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Load failed: {e.Message}");
        }
    }


    private void ClearExistingBuildings()
    {
        var existingBuildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (var building in existingBuildings)
        {
            Destroy(building);
        }
        Debug.Log($"Cleared {existingBuildings.Length} existing buildings.");
    }
}