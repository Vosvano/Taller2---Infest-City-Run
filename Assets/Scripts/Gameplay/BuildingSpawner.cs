using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    // Esta clase muestra como es el desarrollo del spawn de los edificios presentes en el juego, estos se generan de forma aleatoria y contínua
    // al igual que las plataformas

     [Header("Buildings")]
    public List<GameObject> BuildingPrefab = new List<GameObject>();
    public int initialBuildings = 10;
    public float BuildingsLength = 10f;

    [Header("References")]
    public Transform player;
    public enum SpawnAxis { Z, X }
    public SpawnAxis spawnAxis = SpawnAxis.Z;

    private float nextSpawn = 0f;
    private List<GameObject> activeBuildings = new List<GameObject>();
    private int spawnedBuildingCount = 0;

    void Start()
    {

        // Inicializar posición de spawn según el eje y la posición del spawner
        if (spawnAxis == SpawnAxis.Z)
            nextSpawn = transform.position.z;
        else
            nextSpawn = transform.position.x;

        // Generar las plataformas iniciales (sin obstáculos)
        for (int i = 0; i < initialBuildings; i++)
        {
            SpawnBuilding();
        }
    }

    void Update()
    {
        if (player == null || activeBuildings.Count == 0) return;

        // Si el primer building (el más atrás) quedó demasiado atrás respecto al jugador, destruirlo y generar uno nuevo
        GameObject first = activeBuildings[0];
        if (spawnAxis == SpawnAxis.Z)
        {
            if (player.position.z - first.transform.position.z > BuildingsLength)
            {
                Destroy(first);
                activeBuildings.RemoveAt(0);
                SpawnBuilding();
            }
        }
        else // eje X
        {
            if (player.position.x - first.transform.position.x > BuildingsLength)
            {
                Destroy(first);
                activeBuildings.RemoveAt(0);
                SpawnBuilding();
            }
        }
    }

    public void SpawnBuilding()
    {
        spawnedBuildingCount++;

        Vector3 spawnPos = transform.position;
        if (spawnAxis == SpawnAxis.Z)
        {
            spawnPos.z = nextSpawn;
        }
        else
        {
            spawnPos.x = nextSpawn;
        }

        int prefabIndex = Random.Range(0, BuildingPrefab.Count);
        GameObject building = Instantiate(BuildingPrefab[prefabIndex], spawnPos, Quaternion.identity);
        activeBuildings.Add(building);
        nextSpawn += BuildingsLength;
    }
}
