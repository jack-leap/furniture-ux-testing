using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    public static GameObjectManager Instance { get; private set; }

    public static GameObject CurrentlySelectedObject;

    public static event Action<GameObject> OnSelectedObjectChanged;

    private List<GameObject> gameObjects = new List<GameObject>();

    public static int CurrentlySelectedRoom;

    public List<GameObject> room_1_gameobjects = new List<GameObject>();

    public Transform room_1_camera_transform;

    [Space(10)]

    public List<GameObject> room_2_gameobjects = new List<GameObject>();

    public Transform room_2_camera_transform;

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

    private void Start()
    {
        SetRoom(1);
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

    public void RemoveAllGameObject()
    {
        while (gameObjects.Count > 0)
        {
            RemoveLastGameObject();
        }
    }

    public void SetSelectedObject(GameObject selectedObject)
    {
        CurrentlySelectedObject = selectedObject;
        OnSelectedObjectChanged?.Invoke(selectedObject);
    }

    public void SetRoom(int index)
    {        
        if (CurrentlySelectedRoom == index)
        {
            return;
        }
        else if (index == 1)
        {
            foreach (GameObject obj in room_1_gameobjects)
            {
                obj.SetActive(true);
            }

            foreach (GameObject obj in room_2_gameobjects)
            {
                obj.SetActive(false);
            }

            Camera.main.transform.position = room_1_camera_transform.position;
            Camera.main.transform.rotation = room_1_camera_transform.rotation;

            RemoveAllGameObject();

            CurrentlySelectedRoom = 1;
        }
        else if (index == 2)
        {
            foreach (GameObject obj in room_1_gameobjects)
            {
                obj.SetActive(false);
            }

            foreach (GameObject obj in room_2_gameobjects)
            {
                obj.SetActive(true);
            }

            Camera.main.transform.position = room_2_camera_transform.position;
            Camera.main.transform.rotation = room_2_camera_transform.rotation;

            RemoveAllGameObject();

            CurrentlySelectedRoom = 2;
        }
    }
}
