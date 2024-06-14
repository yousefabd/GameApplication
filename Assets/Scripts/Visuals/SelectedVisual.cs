using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedVisual : MonoBehaviour
{
    [SerializeField] private Transform entity;

    private void Start()
    {
        Unit unit = entity.GetComponent<Unit>();
        unit.OnSelect += Unit_OnSelect;
        gameObject.SetActive(false);
    }

    private void Unit_OnSelect(bool selected)
    {
        gameObject.SetActive(selected);
    }
}
