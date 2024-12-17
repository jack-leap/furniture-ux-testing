using UnityEngine;

public class Removable : MonoBehaviour
{
    void OnMouseOver()
    {
        // Check if the right mouse button is pressed (0 = left, 1 = right, 2 = middle)
        if (Input.GetMouseButtonDown(1))
        {
            // Destroy the GameObject this script is attached to
            Destroy(gameObject);
        }
    }
}
