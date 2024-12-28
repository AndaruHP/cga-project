using System.Collections.Generic;
using UnityEngine;

public class RandomizeBattery : MonoBehaviour
{
    [SerializeField] private int numberOfObjectsToRender = 3; // Jumlah objek yang akan dirender
    private List<GameObject> batteryObjects = new List<GameObject>();

    private void Start()
    {
        batteryObjects.AddRange(GameObject.FindGameObjectsWithTag("Battery"));

        if (batteryObjects.Count > 0)
        {
            RandomizeRenderers();
        }
        else
        {
            Debug.LogWarning("Tidak ada objek dengan tag 'Battery' ditemukan!");
        }
    }

    private void RandomizeRenderers()
    {
        // Nonaktifkan semua MeshRenderer
        foreach (var obj in batteryObjects)
        {
            obj.GetComponent<MeshRenderer>().enabled = false;
        }

        // Pilih objek secara acak
        List<int> randomIndices = new List<int>();
        while (randomIndices.Count < numberOfObjectsToRender)
        {
            int randomIndex = Random.Range(0, batteryObjects.Count);
            if (!randomIndices.Contains(randomIndex))
            {
                randomIndices.Add(randomIndex);
            }
        }

        // Aktifkan MeshRenderer untuk objek yang dipilih
        foreach (int index in randomIndices)
        {
            batteryObjects[index].GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
