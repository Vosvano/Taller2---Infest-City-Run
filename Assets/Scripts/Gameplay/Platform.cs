using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Item spawn settings")]
    public Transform[] itemPositions;
    public GameObject[] possibleItems;
    [Tooltip("Pesos opcionales para cada tipo en possibleItems. Si vacío se usa probabilidad uniforme.")]
    public float[] spawnWeights;

    [Range(0f, 1f)]
    public float spawnChance = 0.8f;

    [Header("Quantity Limits")]
    public int minItemsPerPlatform = 1;
    public int maxItemsPerPlatform = 3;

    private static int lastSpawnedItemIndex = -1;

    void Start()
    {
    }

    public void SpawnItemOnPlatform(bool canSpawnObstacles)
    {
        if (!canSpawnObstacles) return;

        if (possibleItems == null || possibleItems.Length == 0) return;
        if (itemPositions == null || itemPositions.Length == 0) return;

        if (Random.value > spawnChance) return;

        int[] allowedIndices = GetAllowedItemIndices(canSpawnObstacles);
        if (allowedIndices == null || allowedIndices.Length == 0) return;

        int targetAmount = Random.Range(minItemsPerPlatform, maxItemsPerPlatform + 1);
        targetAmount = Mathf.Min(targetAmount, itemPositions.Length);

        List<int> availablePosIndices = new List<int>();
        for (int i = 0; i < itemPositions.Length; i++)
        {
            if (itemPositions[i] != null && itemPositions[i].childCount == 0)
            {
                availablePosIndices.Add(i);
            }
        }

        for (int i = 0; i < targetAmount; i++)
        {
            if (availablePosIndices.Count == 0) break;

            int itemIndex = PickWeightedItemIndex(allowedIndices);

            int attempts = 0;
            while (attempts < 5 && itemIndex == lastSpawnedItemIndex && allowedIndices.Length > 1)
            {
                itemIndex = PickWeightedItemIndex(allowedIndices);
                attempts++;
            }

            int randomListIndex = Random.Range(0, availablePosIndices.Count);
            int posIndex = availablePosIndices[randomListIndex];
            availablePosIndices.RemoveAt(randomListIndex);

            if (possibleItems[itemIndex] == null || itemPositions[posIndex] == null) continue;

            Vector3 spawnPos = transform.TransformPoint(itemPositions[posIndex].localPosition);

            // Nacimiento con la rotación del punto de spawn
            GameObject inst = Instantiate(possibleItems[itemIndex], spawnPos, itemPositions[posIndex].rotation, itemPositions[posIndex]);

            Coin coinComponent = inst.GetComponent<Coin>();
            bool isEnemy = inst.GetComponent<Enemy>() != null;
            bool isObstacle = !isEnemy && coinComponent == null;
            bool isVehicle = inst.GetComponent<Vehicle>() != null;

            inst.transform.localPosition = new Vector3(0f, inst.transform.localPosition.y, 0f);

            float alturaFinal = 0f;
            if (isEnemy) alturaFinal = 0.2f;
            else if (isVehicle) alturaFinal = 0f;
            else if (isObstacle) alturaFinal = 0.1f;
            else if (coinComponent != null) alturaFinal = coinComponent.verticalOffset;

            Vector3 posicionGlobalActual = inst.transform.position;
            inst.transform.position = new Vector3(posicionGlobalActual.x, spawnPos.y + alturaFinal, posicionGlobalActual.z);

            if (isObstacle)
            {
                inst.transform.rotation = Quaternion.Euler(-90f, Random.Range(-20f, 20f), 0f);
            }
            else if (isVehicle)
            {
                // Dejarlo como hijo y forzar que sus ángulos locales se reseteen por completo a los del punto de spawn
                inst.transform.localEulerAngles = Vector3.zero;

                // Si el modelo visual está dentro de un hijo, reseteamos su rotación local también
                if (inst.transform.childCount > 0)
                {
                    inst.transform.GetChild(0).localEulerAngles = Vector3.zero;
                }
            }
            else
            {
                inst.transform.rotation = Quaternion.Euler(0f, Random.Range(-20f, 20f), 0f);
            }

            if (isEnemy)
            {
                inst.transform.parent = null;
            }

            lastSpawnedItemIndex = itemIndex;
        }
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