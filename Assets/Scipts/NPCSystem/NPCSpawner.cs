using System;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private GameObject NPC;
    private int previousAmount;
    public void SpawnNPCs(int amount = 1)
    {
        if (amount > previousAmount) {
            int difference = amount - previousAmount;
            for (int i = 0; i < difference; i++) {
                Instantiate(NPC, transform.position, Quaternion.identity);
            }
            previousAmount = amount;
        } else {

        }
        
    }
}
