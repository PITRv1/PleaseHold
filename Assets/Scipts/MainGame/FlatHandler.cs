using System.Collections.Generic;
using UnityEngine;

public class FlatHandler : MonoBehaviour
{
    List<Flat> flatList = new List<Flat>();
    public static FlatHandler Instance {
        private set;
        get;
    }
    private void Awake() {
        Instance = this;
    }

    public void AddToFlatList(Flat flat) {
        flatList.Add(flat);
    }

    public List<Flat> GetFlatList() {
        return flatList;
    }
}
