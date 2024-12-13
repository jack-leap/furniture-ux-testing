using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Plane dragPlane;
    private Vector3 offset;

    public enum DragPlane { CameraParallel, XZ }
    public DragPlane selectedPlane = DragPlane.XZ;

    public Vector3 minLimits = new Vector3(-10f, -10f, -10f); // Minimum limits
    public Vector3 maxLimits = new Vector3(10f, 10f, 10f);    // Maximum limits
    public bool useLimits = true; // Toggle for enabling/disabling limits

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleDragging();
    }

    private void HandleDragging()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                // Begin dragging
                isDragging = true;

                // Set the drag plane based on the selected plane
                if (selectedPlane == DragPlane.CameraParallel)
                {
                    dragPlane = new Plane(mainCamera.transform.forward, transform.position);
                }
                else if (selectedPlane == DragPlane.XZ)
                {
                    dragPlane = new Plane(Vector3.up, transform.position);
                }

                // Calculate offset
                float enter;
                if (dragPlane.Raycast(ray, out enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);
                    offset = transform.position - hitPoint;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Stop dragging
            isDragging = false;
        }

        if (isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            float enter;

            if (dragPlane.Raycast(ray, out enter))
            {
                // Update object position
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 newPosition = hitPoint + offset;

                if (useLimits)
                {
                    // Apply limits
                    newPosition.x = Mathf.Clamp(newPosition.x, minLimits.x, maxLimits.x);
                    newPosition.y = Mathf.Clamp(newPosition.y, minLimits.y, maxLimits.y);
                    newPosition.z = Mathf.Clamp(newPosition.z, minLimits.z, maxLimits.z);
                }

                transform.position = newPosition;
            }
        }
    }
}
