using UnityEngine;
using UnityEngine.SceneManagement;

public class GridPlacement : MonoBehaviour
{
    public float gridSize = 1;
    public LayerMask placementCollisionLayer;
    public GameObject buildingSelectorTab;

    private Vector2 lastClickedPosition;
    private bool awaitingBuildingSelection = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !awaitingBuildingSelection)
        {
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float size = 10f;
        for (float x = -size; x <= size; x += gridSize)
        {
            for (float y = -size; y <= size; y += gridSize)
            {
                Gizmos.DrawLine(new Vector3(x, -size, 0), new Vector3(x, size, 0));
                Gizmos.DrawLine(new Vector3(-size, y, 0), new Vector3(size, y, 0));
            }
        }
    }
}
