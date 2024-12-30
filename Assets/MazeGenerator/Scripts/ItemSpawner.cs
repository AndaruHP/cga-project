using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public GameObject ammoPrefab;
    public GameObject batteryPrefab;
    public GameObject keyPrefab;
    public int numberOfAmmo = 3;
    public int numberOfBatteries = 3;
    public float itemHeight = 0.2f;
    public float minDistanceFromStart = 15f;

    private List<Vector3> usedPositions = new List<Vector3>();

    public void SpawnItems(int rows, int columns, float cellWidth, float cellHeight, bool addGaps)
    {
        bool canSpawn = ValidatePrefabs();
        if (!canSpawn) return;

        // Spawn Key first (to ensure best position)
        SpawnKey(rows, columns, cellWidth, cellHeight, addGaps);

        // Spawn Ammo
        for (int i = 0; i < numberOfAmmo; i++)
        {
            Vector3 position = GetRandomPosition(rows, columns, cellWidth, cellHeight, addGaps);
            GameObject ammo = Instantiate(ammoPrefab, position, Quaternion.identity);
            ammo.transform.parent = transform;
            Debug.Log($"Spawned ammo at position: {position}");
        }

        // Spawn Batteries
        for (int i = 0; i < numberOfBatteries; i++)
        {
            Vector3 position = GetRandomPosition(rows, columns, cellWidth, cellHeight, addGaps);
            GameObject battery = Instantiate(batteryPrefab, position, Quaternion.identity);
            battery.transform.parent = transform;
            Debug.Log($"Spawned battery at position: {position}");
        }
    }

    private void SpawnKey(int rows, int columns, float cellWidth, float cellHeight, bool addGaps)
    {
        if (keyPrefab == null)
        {
            Debug.LogError("Key Prefab is not assigned!");
            return;
        }

        Vector3 position;
        bool validPosition = false;
        int attempts = 0;
        int maxAttempts = 100;

        do
        {
            position = GetRandomPosition(rows, columns, cellWidth, cellHeight, addGaps);
            // Check if position is far enough from start (0,0,0)
            validPosition = Vector3.Distance(position, Vector3.zero) >= minDistanceFromStart;
            attempts++;

            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Could not find ideal key position after " + maxAttempts + " attempts. Using last position.");
                break;
            }

        } while (!validPosition);

        GameObject key = Instantiate(keyPrefab, position, Quaternion.identity);
        key.transform.parent = transform;
        usedPositions.Add(position);
        Debug.Log($"Spawned key at position: {position}, Distance from start: {Vector3.Distance(position, Vector3.zero)}");
    }

    private bool ValidatePrefabs()
    {
        bool isValid = true;

        if (ammoPrefab == null)
        {
            Debug.LogError("Ammo Prefab is not assigned in the MazeSpawner component!");
            isValid = false;
        }

        if (batteryPrefab == null)
        {
            Debug.LogError("Battery Prefab is not assigned in the MazeSpawner component!");
            isValid = false;
        }

        if (keyPrefab == null)
        {
            Debug.LogError("Key Prefab is not assigned in the MazeSpawner component!");
            isValid = false;
        }

        return isValid;
    }

    private Vector3 GetRandomPosition(int rows, int columns, float cellWidth, float cellHeight, bool addGaps)
    {
        Vector3 position;
        bool validPosition = false;
        int maxAttempts = 50;
        int attempts = 0;

        do
        {
            float gapOffset = addGaps ? 0.2f : 0f;
            float x = Random.Range(0, columns) * (cellWidth + gapOffset);
            float z = Random.Range(0, rows) * (cellHeight + gapOffset);
            position = new Vector3(x, itemHeight, z);

            // Check if position is far enough from other items
            validPosition = IsValidPosition(position);
            attempts++;

        } while (!validPosition && attempts < maxAttempts);

        usedPositions.Add(position);
        return position;
    }

    private bool IsValidPosition(Vector3 position)
    {
        foreach (Vector3 usedPosition in usedPositions)
        {
            if (Vector3.Distance(position, usedPosition) < 2f) // Minimum distance between items
            {
                return false;
            }
        }
        return true;
    }
}