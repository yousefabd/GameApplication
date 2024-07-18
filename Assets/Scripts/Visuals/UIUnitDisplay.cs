using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitDisplay : MonoBehaviour
{
    public static UIUnitDisplay Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void createButtons(List<UnitSO> unitSOList, Building building)
    {
        UIManager.Instance.SwitchContent(true);
        for (int i = 0; i <unitSOList.Count; i++)
        {
            UnitSO unitSO = unitSOList[i];
            GameObject buttonInstance = Instantiate(unitSO.prefab.gameObject);
            Button button = buttonInstance.AddComponent<Button>();
            button.onClick.AddListener(() => building.Spawner(unitSO));
        }
    }
}
