using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private GridPlacement gridPlacement;
    [SerializeField] private GameObject buildingPrefab;

    public void OnBuildingButtonClicked()
    {
        gridPlacement.PlaceSelectedBuilding(buildingPrefab);
    }
}