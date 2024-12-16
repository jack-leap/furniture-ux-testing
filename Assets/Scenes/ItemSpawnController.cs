using UnityEngine;

public class ItemSpawnController : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnRoot;

    public void Spawn()
    {
        Instantiate(prefabToSpawn, spawnRoot);
    }
}
