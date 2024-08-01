using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Transform mainMenuList;
    [SerializeField] private Transform gameModesList;
    public static MainMenuUI Instance;
    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }
    private void Start()
    {
        SelectMainMenuList();
    }
    public void LoadRealTimeStrategy()
    {
        Loader.Load(Loader.Scene.PathfindingSystem1);
    }
    public void LoadTowerDefense()
    {
        Loader.Load(Loader.Scene.TowerDefenseMode);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void SelectMainMenuList()
    {
        gameModesList.gameObject.SetActive(false);
        mainMenuList.gameObject.SetActive(true);
    }
    public void SelectGameModesList()
    {
        gameModesList.gameObject.SetActive(true);
        mainMenuList.gameObject.SetActive(false);
    }

}