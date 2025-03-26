using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventDisplay : MonoBehaviour
{
    [SerializeField] private Animator uiAnimator;
    [SerializeField] private Button hideButton;
    [SerializeField] private TextMeshProUGUI eventName;
    [SerializeField] private TextMeshProUGUI eventText;


    private void Awake()
    {
        uiAnimator = GetComponent<Animator>();
        hideButton.onClick.AddListener(HideUI);
    }

    public void ShowUI()
    {
        uiAnimator.Play("SlideInOut");
    }

    private void HideUI()
    {
        uiAnimator.Play("SlideOut");
    }

    public void SetEventName(string name)
    {
        eventName.text = name;
    }
    public void SetEventText(string text)
    {
        eventText.text = text;
    }

    public void SetEventColor(Color color)
    {
        eventText.color = color;
    }
}
