using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingDisplay : MonoBehaviour
{
    public static UIBuildingDisplay Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] BuildingSOList buildingsList;

    private void Start()
    {
        createButtons();
    }

    void createButtons()
    {
        for (int i = 0; i < buildingsList.buildingSOList.Count; i++)
        {
            BuildingSO buildingSO = buildingsList.buildingSOList[i];
            GameObject buttonInstance = Instantiate(buildingSO.buildingPrefab.gameObject);
            Button button = buttonInstance.AddComponent<Button>();
            button.onClick.AddListener(() => BuildingManager.Instance.UIHelper(buildingSO));
        }
    }
}
