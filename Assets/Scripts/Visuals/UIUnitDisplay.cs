using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitDisplay : MonoBehaviour
{
    [SerializeField] private Transform UIParent;
    [SerializeField] private GameObject UIButton;

    public static UIUnitDisplay Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
    }
    public void createButtons(List<UnitSO> unitSOList, Building building)
    {
        Debug.Log("entered");
        UIManager.Instance.SwitchContent(true);

        foreach (UnitSO unitSO in unitSOList)
        {
            GameObject buttonInstance = Instantiate(UIButton, UIParent);
            Button button = buttonInstance.GetComponent<Button>();

            // Set the button's image sprite
            Image buttonImage = buttonInstance.transform.GetChild(0).GetComponent<Image>();
            buttonImage.sprite = unitSO.icon;

            // Add listener for button click
            button.onClick.AddListener(() => building.Spawner(unitSO));
        }
    }

}
