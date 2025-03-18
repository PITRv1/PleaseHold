using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class SkipTurn : MonoBehaviour {

    [SerializeField] Button newMonthButton;

    private void Awake() {
        newMonthButton.onClick.AddListener(() => {
            GameHandler.Instance.NewMonth();
        });
    }

}
