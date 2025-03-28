using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private GameObject NPC;
    private int previousAmount;

    public void SpawnNPCs(int amount = 1)
    {
        if (amount >= previousAmount) 
        {
            int difference = amount - previousAmount;
            for (int i = 0; i < difference; i++) 
            {
                Instantiate(NPC, transform.position, Quaternion.identity);
            }
            previousAmount = amount;
        } else {
            int difference = previousAmount - amount;
            List<GameObject> npcs = GetGameObjectsFromLayer("NPC");

            for (int i = 0; i < difference; i++) 
            {
                Destroy(npcs[i]);
            }
        }
    }

    private List<GameObject> GetGameObjectsFromLayer(string layerName)
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        List<GameObject> objectsInLayer = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer(layerName))
            {
                objectsInLayer.Add(obj);
            }
        }

        return objectsInLayer;
    }
}
