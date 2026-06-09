using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Item spawn settings")]
    public Transform[] itemPositions; // 3 posibles posiciones
    public GameObject[] possibleItems; // moneda, enemigo, obstáculo
    [Tooltip("Pesos opcionales para cada tipo en possibleItems. Si vacío se usa probabilidad uniforme.")]
    public float[] spawnWeights;

    [Range(0f, 1f)]
    public float spawnChance = 0.8f; // probabilidad de que aparezca algún item

    // para evitar repetición monotona entre plataformas
    private static int lastSpawnedItemIndex = -1;

    void Start()
    {
    }

    public void SpawnItemOnPlatform(bool canSpawnObstacles)
    {
        if (possibleItems == null || possibleItems.Length == 0) return;
        if (itemPositions == null || itemPositions.Length == 0) return;

        if (Random.value > spawnChance) return; // no spawnear nada

        int[] allowedIndices = GetAllowedItemIndices(canSpawnObstacles);
        if (allowedIndices == null || allowedIndices.Length == 0) return;

        int itemIndex = PickWeightedItemIndex(allowedIndices);
        int posIndex = Random.Range(0, itemPositions.Length);

        if (possibleItems[itemIndex] == null || itemPositions[posIndex] == null) return;

        // si el ultimo spawn fue del mismo tipo, reintentar para variar (hasta 5 intentos)
        int attempts = 0;
        while (attempts < 5 && itemIndex == lastSpawnedItemIndex && allowedIndices.Length > 1)
        {
            itemIndex = PickWeightedItemIndex(allowedIndices);
            attempts++;
        }

        // calcular posición de spawn usando la posición local del punto
        Vector3 spawnPos = transform.TransformPoint(itemPositions[posIndex].localPosition);

        // evitar solapamientos si el punto ya tiene un hijo
        if (itemPositions[posIndex].childCount > 0) return;

        // usar la rotación del prefab como base para que las rotaciones aplicadas en el prefab se respeten
        Quaternion prefabWorldRot = possibleItems[itemIndex].transform.rotation;
        GameObject inst = Instantiate(possibleItems[itemIndex], spawnPos, prefabWorldRot, itemPositions[posIndex]);

        // aplicar la rotación base del prefab + una variación en Y para evitar que queden idénticos
        Vector3 baseLocalEuler = possibleItems[itemIndex].transform.localEulerAngles;
        inst.transform.localRotation = Quaternion.Euler(baseLocalEuler + new Vector3(0f, Random.Range(-20f, 20f), 0f));
        inst.transform.localPosition = new Vector3(0f, inst.transform.localPosition.y, 0f);

        // si es un enemigo, lo desemparentamos para que se mueva independientemente
        if (inst.GetComponent<Enemy>() != null)
        {
            inst.transform.parent = null;
        }

        lastSpawnedItemIndex = itemIndex;
    }

    private int[] GetAllowedItemIndices(bool canSpawnObstacles)
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < possibleItems.Length; i++)
        {
            GameObject prefab = possibleItems[i];
            if (prefab == null) continue;

            if (!canSpawnObstacles && IsObstaclePrefab(prefab))
            {
                continue;
            }

            indices.Add(i);
        }
        return indices.ToArray();
    }

    private int PickWeightedItemIndex(int[] allowedIndices)
    {
        if (allowedIndices == null || allowedIndices.Length == 0) return -1;

        // Si no hay pesos válidos, elegir uniforme
        if (spawnWeights == null || spawnWeights.Length != possibleItems.Length)
        {
            return allowedIndices[Random.Range(0, allowedIndices.Length)];
        }

        float total = 0f;
        for (int i = 0; i < allowedIndices.Length; i++)
        {
            total += Mathf.Max(0f, spawnWeights[allowedIndices[i]]);
        }

        if (total <= 0f)
        {
            return allowedIndices[Random.Range(0, allowedIndices.Length)];
        }

        float r = Random.value * total;
        float accum = 0f;
        for (int i = 0; i < allowedIndices.Length; i++)
        {
            int index = allowedIndices[i];
            accum += Mathf.Max(0f, spawnWeights[index]);
            if (r <= accum)
            {
                return index;
            }
        }

        return allowedIndices[allowedIndices.Length - 1];
    }

    private bool IsObstaclePrefab(GameObject prefab)
    {
        if (prefab == null) return false;

        if (prefab.GetComponent<Obstacle>() != null)
        {
            return true;
        }

        string lowerName = prefab.name.ToLowerInvariant();
        return lowerName.Contains("obstacle") ||
               lowerName.Contains("obstac") ||
               lowerName.Contains("barricade") ||
               lowerName.Contains("trash") ||
               lowerName.Contains("box");
    }
}
