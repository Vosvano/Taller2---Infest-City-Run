using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Platform Settings")]
    public GameObject platformPrefab;
    public int initialPlatforms = 12;
    public float platformLength = 10f;

    [Header("References")]
    public Transform player;
    public enum SpawnAxis { Z, X }
    public SpawnAxis spawnAxis = SpawnAxis.Z;

    private float nextSpawn = 0f;
    private List<GameObject> activePlatforms = new List<GameObject>();
    private int spawnedPlatformCount = 0;

    void Start()
    {
        if (platformPrefab == null)
        {
            Debug.LogError("PlatformSpawner: platformPrefab no está asignado.");
            return;
        }

        // Inicializar posición de spawn según el eje y la posición del spawner
        if (spawnAxis == SpawnAxis.Z)
            nextSpawn = transform.position.z;
        else
            nextSpawn = transform.position.x;

        // Generar las plataformas iniciales (sin obstáculos)
        for (int i = 0; i < initialPlatforms; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        if (player == null || activePlatforms.Count == 0) return;

        // Si el primer platform (el más atrás) quedó demasiado atrás respecto al jugador, destruirlo y generar uno nuevo
        GameObject first = activePlatforms[0];
        if (spawnAxis == SpawnAxis.Z)
        {
            if (player.position.z - first.transform.position.z > platformLength)
            {
                Destroy(first);
                activePlatforms.RemoveAt(0);
                SpawnPlatform();
            }
        }
        else // eje X
        {
            if (player.position.x - first.transform.position.x > platformLength)
            {
                Destroy(first);
                activePlatforms.RemoveAt(0);
                SpawnPlatform();
            }
        }
    }

    public void SpawnPlatform()
    {
        spawnedPlatformCount++;

        Vector3 spawnPos = transform.position;
        if (spawnAxis == SpawnAxis.Z)
        {
            spawnPos.z = nextSpawn;
        }
        else
        {
            spawnPos.x = nextSpawn;
        }

        GameObject plat = Instantiate(platformPrefab, spawnPos, Quaternion.identity);
        Platform platform = plat.GetComponent<Platform>();
        if (platform != null)
        {
            // Las primeras plataformas salen limpias; después ya pueden aparecer obstáculos.
            bool canSpawnObstacles = spawnedPlatformCount > initialPlatforms;
            platform.SpawnItemOnPlatform(canSpawnObstacles);
        }
        activePlatforms.Add(plat);
        nextSpawn += platformLength;
    }
}
