using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using NUnit.Framework;
using System;

public class PlaySubMenu : SubMenu
{
    public event EventHandler OnSimStarted;


    [SerializeField] private Button simulationStartButton;

    public void Awake()
    {
        simulationStartButton.onClick.AddListener(() =>
        {
            OnSimStarted?.Invoke(this, EventArgs.Empty);
        });
    }

}