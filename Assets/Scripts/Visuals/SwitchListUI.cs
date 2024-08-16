using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchListUI : MonoBehaviour
{
    [SerializeField] Transform MainMenu;
    [SerializeField] Transform GameModes;
    public static SwitchListUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DeactivateAll();
        MainMenu.gameObject.SetActive(true);
    }
    private void DeactivateAll()
    {
        MainMenu.gameObject.SetActive(false);
        GameModes.gameObject.SetActive(false);
    }
    public void ShowGameModes()
    {
        DeactivateAll();
        GameModes.gameObject.SetActive(true);
    }
    public void BackToMainMenu()
    {
        DeactivateAll();
        MainMenu.gameObject.SetActive(true);
    }

}
