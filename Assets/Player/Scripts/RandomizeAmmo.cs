using System.Collections.Generic;
using UnityEngine;

public class RandomizeAmmo : MonoBehaviour
{
    [SerializeField] private int numberOfObjectsToRender = 10; // Jumlah objek yang akan dirender
    private List<GameObject> boxAmmoObjects = new List<GameObject>();

    private void Start()
    {
        // Cari semua objek dengan tag "Ammo"
        boxAmmoObjects.AddRange(GameObject.FindGameObjectsWithTag("Ammo"));

        if (boxAmmoObjects.Count > 0)
        {
            RandomizeRenderers();
        }
        else
        {
            Debug.LogWarning("Tidak ada objek dengan tag 'Ammo' ditemukan!");
        }
    }

    private void RandomizeRenderers()
    {
        // Nonaktifkan semua MeshRenderer
        foreach (var obj in boxAmmoObjects)
        {
            obj.GetComponent<MeshRenderer>().enabled = false;
        }

        // Pilih objek secara acak
        List<int> randomIndices = new List<int>();
        while (randomIndices.Count < numberOfObjectsToRender)
        {
            int randomIndex = Random.Range(0, boxAmmoObjects.Count);
            if (!randomIndices.Contains(randomIndex))
            {
                randomIndices.Add(randomIndex);
            }
        }

        // Aktifkan MeshRenderer untuk objek yang dipilih
        foreach (int index in randomIndices)
        {
            boxAmmoObjects[index].GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
