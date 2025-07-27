using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask placementLayerMask;
    private Vector3 lastPosition;

    public Vector3 GetSelectedMapPosition()
    {
        if (sceneCamera == null)
            return lastPosition;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -sceneCamera.transform.position.z;
        Vector2 rayOrigin = sceneCamera.ScreenToWorldPoint(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero, Mathf.Infinity, placementLayerMask);

        if (hit.collider != null)
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }

}
