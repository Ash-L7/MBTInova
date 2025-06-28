using UnityEngine;

public class GridPlacement : MonoBehaviour
{
    public float gridSize = 1;
    public GameObject infrastructureToPlaceDown;
    
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

        Instantiate(infrastructureToPlaceDown, snappedMousePos, Quaternion.identity);
    }
}
