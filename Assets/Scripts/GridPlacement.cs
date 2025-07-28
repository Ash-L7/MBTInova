using UnityEngine;
using UnityEngine.SceneManagement;

public class GridPlacement : MonoBehaviour
{
    public float gridSize = 1;
    public LayerMask placementCollisionLayer;
    public GameObject buildingSelectorTab;
    public GameObject infrastructureToPlaceDown;
    private Vector2 lastClickedPosition;
    private bool awaitingBuildingSelection = false;

    bool IsPointerOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !awaitingBuildingSelection)
        {
            if (IsPointerOverUI()) return;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 snappedMousePos = new Vector2(Mathf.Round(mousePos.x / gridSize) * gridSize,
                                                  Mathf.Round(mousePos.y / gridSize) * gridSize);

            if (CanPlaceAtPosition(snappedMousePos))
            {
                lastClickedPosition = snappedMousePos;
                awaitingBuildingSelection = true;
                buildingSelectorTab.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene(2);
            }
        }
    }

    public void PlaceSelectedBuilding(GameObject prefab)
    {
        if (!awaitingBuildingSelection || prefab == null)
            return;

        GameObject placed = Instantiate(prefab, lastClickedPosition, Quaternion.identity);
        var identifier = placed.AddComponent<BuildingIdentifier>();
        identifier.prefabName = prefab.name;
        placed.tag = "Building";

        buildingSelectorTab.SetActive(false);
        awaitingBuildingSelection = false;
    }

    private bool CanPlaceAtPosition(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, new Vector2(gridSize * 0.9f, gridSize * 0.9f), 0f, placementCollisionLayer);
        return colliders.Length == 0;
    }

    public void SetCurrentBuilding(GameObject buildingPrefab)
    {
        infrastructureToPlaceDown = buildingPrefab;
    }

    public void CancelBuildingPlacement()
    {
        buildingSelectorTab.SetActive(false);
        FindObjectOfType<GridPlacement>().SetCurrentBuilding(null);
        awaitingBuildingSelection = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        float size = 10f;
        for (float x = -size; x <= size; x += gridSize)
        {
            Gizmos.DrawLine(new Vector3(x, -size, 0), new Vector3(x, size, 0));
            Gizmos.DrawLine(new Vector3(-size, x, 0), new Vector3(size, x, 0));
        }

        if (buildingSelectorTab != null && buildingSelectorTab.activeSelf)
            return;

        if (Camera.main != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 snappedPos = new Vector2(
                Mathf.Round(mouseWorld.x / gridSize) * gridSize,
                Mathf.Round(mouseWorld.y / gridSize) * gridSize
            );

            Vector3 center = new Vector3(snappedPos.x, snappedPos.y, 0f);
            Vector3 sizeVec = new Vector3(gridSize, gridSize, 0f);

            Gizmos.color = new Color(1f, 1f, 0f, 0.4f);
            Gizmos.DrawCube(center, sizeVec);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(center, sizeVec);
        }
    }
}
