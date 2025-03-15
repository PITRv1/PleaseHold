using UnityEngine;

public class SelectType : MonoBehaviour {

    public static SelectType Instance {
        private set;
        get;
    }
    private void Awake() {
        Instance = this;
        
    }
    
}
