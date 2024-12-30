using UnityEngine;
using UnityEngine.UI;

public class ObjectRotator : MonoBehaviour
{
    public Slider rotationSlider;  // Reference to the UI Slider

    private void OnEnable()
    {
        // Subscribe to the event
        GameObjectManager.OnSelectedObjectChanged += UpdateSelectedObject;
        if (rotationSlider != null)
        {
            rotationSlider.value = 0;
            rotationSlider.onValueChanged.AddListener(OnRotationValueChanged);
        }
        rotationSlider.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        GameObjectManager.OnSelectedObjectChanged -= UpdateSelectedObject;

        if (rotationSlider != null)
        {
            rotationSlider.onValueChanged.RemoveListener(OnRotationValueChanged);
        }
    }

    private void UpdateSelectedObject(GameObject selectedObject)
    {
        if (rotationSlider != null)
        {
            if (selectedObject != null && selectedObject.GetComponent<Rotateble>() != null)
            {
                // Show the slider and update its value
                rotationSlider.gameObject.SetActive(true);
                float currentYRotation = NormalizeRotation(selectedObject.transform.eulerAngles.y);
                rotationSlider.value = currentYRotation;
            }
            else
            {
                // Hide the slider if no rotatable object is selected
                rotationSlider.gameObject.SetActive(false);
            }
        }
    }

    public void OnRotationValueChanged(float value)
    {
        // Rotate the currently selected object
        if (GameObjectManager.CurrentlySelectedObject != null)
        {
            GameObjectManager.CurrentlySelectedObject.transform.rotation = Quaternion.Euler(0, value, 0);
        }
    }

    private float NormalizeRotation(float rotation)
    {
        // Ensures rotation is in the range 0 to 360
        if (rotation < 0)
        {
            return rotation + 360f;
        }
        return rotation;
    }
}