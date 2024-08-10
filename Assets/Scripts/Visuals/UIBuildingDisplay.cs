using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingDisplay : MonoBehaviour
{
    public static UIBuildingDisplay Instance;
    [SerializeField] private Transform UIParent;
    [SerializeField] private GameObject UIButton;

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
            UIButton.transform.GetChild(0).GetComponent<Image>().sprite = buildingSO.buildingSprite;
            UIButton.transform.GetComponent<Image>().sprite = null;
            ButtonTextController buttonText = UIButton.transform.GetChild(1).GetComponent<ButtonTextController>();
            buttonText.buildingType = buildingSO.buildingType;
            buttonText.buildingPrice = buildingSO.price;
            GameObject buttonInstance = Instantiate(UIButton, UIParent);
            Button button = buttonInstance.AddComponent<Button>();
            button.onClick.AddListener(() => BuildingManager.Instance.UIHelper(buildingSO));    
        }
    }
}
