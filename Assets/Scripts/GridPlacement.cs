using UnityEngine;
using UnityEngine.SceneManagement;

public class GridPlacement : MonoBehaviour
{
    public float gridSize = 1;
    public GameObject infrastructureToPlaceDown;
    public LayerMask placementCollisionLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceItem();
        }
    }

    private void PlaceItem()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 snappedMousePos = new Vector2(Mathf.Round(mousePos.x / gridSize) * gridSize,
                                              Mathf.Round(mousePos.y / gridSize) * gridSize);

        if (CanPlaceAtPosition(snappedMousePos))
        {
            GameObject placed = Instantiate(infrastructureToPlaceDown, snappedMousePos, Quaternion.identity);
            var identifier = placed.AddComponent<BuildingIdentifier>();
            identifier.prefabName = infrastructureToPlaceDown.name;
            placed.tag = "Building";

        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }

    private bool CanPlaceAtPosition(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, new Vector2(gridSize * 0.9f, gridSize * 0.9f), 0f, placementCollisionLayer);
        return colliders.Length == 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Draw grid lines in the scene view
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
