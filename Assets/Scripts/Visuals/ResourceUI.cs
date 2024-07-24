using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
private static ResourceUI instance;

    public GameObject goldObject, woodObject, stoneObject;
    private TMP_Text gold,wood,stone;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        gold = goldObject.GetComponentInChildren<TMP_Text>();
        wood = woodObject.GetComponentInChildren<TMP_Text>();
        stone = stoneObject.GetComponentInChildren<TMP_Text>();
    }
    private void Update()
    {
        gold.text = ResourceManager.Instance.GetGoldAmount().ToString();
        wood.text = ResourceManager.Instance.GetWoodAmount().ToString();
        stone.text = ResourceManager.Instance.GetStoneAmount().ToString();
    }

}
