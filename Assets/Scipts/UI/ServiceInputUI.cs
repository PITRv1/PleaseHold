using TMPro;
using UnityEngine;

public class ServiceInputUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI serviceName;
    [SerializeField] private TMP_InputField costInputField;

    private void Awake()
    {
        costInputField.onEndEdit.AddListener(ValidateInput);
    }

    private void Start()
    {
        serviceName.text = "ServiceName";
    }

    public void SetName(string name)
    {
        this.name = name;
        serviceName.text = name;
    }

    private void ValidateInput(string input)
    {
        if (int.TryParse(input, out int length))
        {
            length = Mathf.Max(length, 1);
            costInputField.text = length.ToString();
        }
        else
        {
            costInputField.text = "1";
        }
    }

    public float GetInputValue()
    {
        return float.Parse(costInputField.text);
    }
}
