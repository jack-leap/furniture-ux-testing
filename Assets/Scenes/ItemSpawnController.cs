using UnityEngine;

public class ItemSpawnController : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnRoot;

    public void Spawn()
    {
        GameObject newObject = Instantiate(prefabToSpawn, spawnRoot);
        if (GameObjectManager.Instance != null)
        {
            GameObjectManager.Instance.RegisterGameObject(newObject);
        }
    }
}
