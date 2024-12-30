using UnityEngine;

public class RotateOnDrag : MonoBehaviour
{
    public float rotationSpeed = 100.0f; // Speed of rotation

    private bool isDragging = false;

    void Update()
    {
        // Check for input
        if (isDragging)
        {
            // Calculate rotation based on mouse movement
            float rotationAmount = -Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

            // Apply rotation to the object's y-axis
            transform.parent.Rotate(0, rotationAmount, 0);
        }
    }

    void OnMouseDown()
    {
        // Activate dragging when the object is clicked
        isDragging = true;
    }

    void OnMouseUp()
    {
        // Deactivate dragging when the mouse button is released
        isDragging = false;
    }
}
