using System;
using UnityEngine;
using UnityEngine.UI;

public class MainButtonUI : MonoBehaviour
{
    public static MainButtonUI Instance { get; private set; }

    public event EventHandler OnPlayMenuButtonClick;
    public event EventHandler OnOptionsMenuButtonClick;
    public event EventHandler OnCreditsMenuButtonClick;


    [SerializeField] private Button playMenuButton;
    [SerializeField] private Button optionsMenuButton;
    [SerializeField] private Button creditsMenuButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        Instance = this;

        playMenuButton.onClick.AddListener(() =>
        {
            OnPlayMenuButtonClick?.Invoke(this, EventArgs.Empty);
        });

        optionsMenuButton.onClick.AddListener(() =>
        {
            OnOptionsMenuButtonClick?.Invoke(this, EventArgs.Empty);
        });

        creditsMenuButton.onClick.AddListener(() =>
        {
            OnCreditsMenuButtonClick?.Invoke(this, EventArgs.Empty);
        });


        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
