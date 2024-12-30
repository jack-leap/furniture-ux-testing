using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    public static GameObjectManager Instance { get; private set; }

    public static GameObject CurrentlySelectedObject;

    public static event Action<GameObject> OnSelectedObjectChanged;

    private List<GameObject> gameObjects = new List<GameObject>();

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: Persist across scenes
    }

    // Method to add a game object to the manager
    public void RegisterGameObject(GameObject newObject)
    {
        gameObjects.Add(newObject);
        if (CurrentlySelectedObject != newObject)
            SetSelectedObject(newObject);
    }

    // Method to remove the last added game object
    public void RemoveLastGameObject()
    {
        if (gameObjects.Count > 0)
        {
            GameObject lastObject = gameObjects[gameObjects.Count - 1];
            gameObjects.RemoveAt(gameObjects.Count - 1);
            Destroy(lastObject); // Destroy the game object
        }

        GameObject selectedObject = gameObjects != null && gameObjects.Count > 0
            ? gameObjects[gameObjects.Count - 1]
            : null;

        SetSelectedObject(selectedObject);
    }

    public void SetSelectedObject(GameObject selectedObject)
    {
        CurrentlySelectedObject = selectedObject;
        OnSelectedObjectChanged?.Invoke(selectedObject);
    }
}
