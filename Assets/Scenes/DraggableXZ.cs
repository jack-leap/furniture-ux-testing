using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Plane dragPlane;
    private Vector3 offset;
    private Rigidbody rb;

    public enum DragPlane { CameraParallel, XZ }
    public DragPlane selectedPlane = DragPlane.XZ;

    public bool useLimits = true; // Toggle for enabling/disabling limits
    public Vector3 minLimits = new Vector3(-10f, -10f, -10f); // Minimum limits
    public Vector3 maxLimits = new Vector3(10f, 10f, 10f);    // Maximum limits
    public LayerMask collisionLayer; // LayerMask to check for obstacles
    public LayerMask draggableLayer; // LayerMask for selectable draggable objects

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity for drag-only behavior
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

            // Raycast only for objects on the draggable layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, draggableLayer))
            {
                if (hit.transform == transform)
                {
                    GameObjectManager.Instance.SetSelectedObject(gameObject);
                    isDragging = true;

                    // Set the drag plane
                    if (selectedPlane == DragPlane.CameraParallel)
                    {
                        dragPlane = new Plane(mainCamera.transform.forward, transform.position);
                    }
                    else if (selectedPlane == DragPlane.XZ)
                    {
                        dragPlane = new Plane(Vector3.up, transform.position);
                    }

                    float enter;
                    if (dragPlane.Raycast(ray, out enter))
                    {
                        Vector3 hitPoint = ray.GetPoint(enter);
                        offset = transform.position - hitPoint;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            rb.linearVelocity = Vector3.zero; // Stop movement when not dragging
        }
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            float enter;

            if (dragPlane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 targetPosition = hitPoint + offset;

                if (useLimits)
                {
                    targetPosition.x = Mathf.Clamp(targetPosition.x, minLimits.x, maxLimits.x);
                    targetPosition.y = Mathf.Clamp(targetPosition.y, minLimits.y, maxLimits.y);
                    targetPosition.z = Mathf.Clamp(targetPosition.z, minLimits.z, maxLimits.z);
                }

                // Check for collisions at the target position
                Collider[] colliders = Physics.OverlapBox(
                    targetPosition,
                    transform.localScale / 2,
                    Quaternion.identity,
                    collisionLayer
                );

                if (colliders.Length == 0)
                {
                    // No collision, move the object
                    rb.MovePosition(targetPosition);
                }
                else
                {
                    // Collision detected, do not move
                    rb.linearVelocity = Vector3.zero;
                }
            }
        }
    }
}
