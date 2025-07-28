using UnityEngine;
using UnityEngine.SceneManagement;

public class GridPlacement : MonoBehaviour
{
    public float gridSize = 1;
    public LayerMask placementCollisionLayer;
    public GameObject buildingSelectorTab;
    public GameObject infrastructureToPlaceDown;

    [Header("Runtime Highlight")]
    public GameObject highlightPrefab; 
    private GameObject currentHighlight;

    private Vector2 lastClickedPosition;
    private bool awaitingBuildingSelection = false;

    bool IsPointerOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    void Start()
    {
        if (highlightPrefab == null)
        {
            CreateDefaultHighlight();
        }

        if (highlightPrefab != null)
        {
            currentHighlight = Instantiate(highlightPrefab);
            currentHighlight.name = "GridHighlight";
            currentHighlight.SetActive(false);
        }
    }

    void CreateDefaultHighlight()
    {
        GameObject highlight = GameObject.CreatePrimitive(PrimitiveType.Quad);
        highlight.name = "HighlightPrefab";

        DestroyImmediate(highlight.GetComponent<Collider>());

        Renderer renderer = highlight.GetComponent<Renderer>();
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = new Color(1f, 1f, 0f, 0.4f);
        renderer.material = mat;

        highlight.transform.localScale = new Vector3(gridSize, gridSize, 1f);

        highlightPrefab = highlight;
    }

    void Update()
    {
        UpdateHighlight();

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

    void UpdateHighlight()
    {
        if (currentHighlight == null) return;

        if (buildingSelectorTab != null && buildingSelectorTab.activeSelf)
        {
            currentHighlight.SetActive(false);
            return;
        }

        if (Camera.main != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 snappedPos = new Vector2(
                Mathf.Round(mouseWorld.x / gridSize) * gridSize,
                Mathf.Round(mouseWorld.y / gridSize) * gridSize
            );

            currentHighlight.transform.position = new Vector3(snappedPos.x, snappedPos.y, -0.1f);
            currentHighlight.SetActive(true);
        }
        else
        {
            currentHighlight.SetActive(false);
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
}
