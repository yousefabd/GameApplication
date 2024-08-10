using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIResource : MonoBehaviour
{
    public TMP_Text Gold;
    public TMP_Text Wood;
    public TMP_Text Stone;
    private void Start()
    {
        ResourceManager.resourceChanged += setAmounts;   
        setAmounts();
    }

    private void setAmounts()
    {
        Gold.text = ResourceManager.Instance.getGoldResource().ToString();
        Wood.text = ResourceManager.Instance.getWoodResource().ToString();
        Stone.text = ResourceManager.Instance.getStoneResource().ToString();
    }


}
