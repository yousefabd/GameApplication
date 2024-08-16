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

  
    public void createButtons(List<UnitSO> unitSOList, Building building)
    {
        //Debug.Log("entered");
        //UIManager.Instance.SwitchContent(true); 

        foreach (UnitSO unitSO in unitSOList)
        {
            UIButton.transform.GetChild(0).GetComponent<Image>().sprite = unitSO.icon;
            UIButton.transform.GetComponent<Image>().sprite = null;

            UnitButtonTextController buttonText = UIButton.transform.GetChild(1).GetComponent<UnitButtonTextController>();
            Debug.Log(unitSO.soldierType);
            buttonText.soldierType = unitSO.soldierType;
            
            buttonText.unitPrice = unitSO.price;
            Debug.Log(Player.currentMaxCount[unitSO.soldierType]);
            Debug.Log(Player.Instance.currentCount[unitSO.soldierType]);
            Debug.Log(unitSO.price);



            GameObject buttonInstance = Instantiate(UIButton, UIParent);
            Button button = buttonInstance.AddComponent<Button>();
            Debug.Log(button);
            button.onClick.AddListener(() => building.Spawner(unitSO));
        }
    }

}
