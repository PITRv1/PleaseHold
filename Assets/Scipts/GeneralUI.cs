using TMPro;
using UnityEngine;

public class GeneralUI : MonoBehaviour {

    [SerializeField] TextMeshProUGUI turnCountText;

    public static GeneralUI Instance {
        private set;
        get;
    }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GameHandler.Instance.changeTurnCountUI += GameHandler_changeTurnCountUI;
    }

    private void GameHandler_changeTurnCountUI(object sender, GameHandler.UI e) {
        turnCountText.text = e.turnCount.ToString();
    }
}
