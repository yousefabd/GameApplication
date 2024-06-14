using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedVisual : MonoBehaviour
{
    [SerializeField] private Transform entity;

    private void Start()
    {
        Unit character = entity.GetComponent<Unit>();
        character.OnSelect += Character_OnSelect;
        gameObject.SetActive(false);
    }

    private void Character_OnSelect(bool selected)
    {
        gameObject.SetActive(selected);
    }
}
