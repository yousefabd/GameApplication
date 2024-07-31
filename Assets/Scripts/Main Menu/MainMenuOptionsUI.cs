using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuOptionsUI : MonoBehaviour
{
    [SerializeField] private Button Play;
    [SerializeField] private Button Settings;
    [SerializeField] private Button Exit;

    private void Start()
    {
        Play.onClick.AddListener(() =>
        {
            MainMenuUI.Instance.SelectGameModesList();
        });
        Exit.onClick.AddListener(() =>
        {
            MainMenuUI.Instance.Quit();
        });
    }
    
}
