using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedVisual : MonoBehaviour
{
    [SerializeField] private Transform entity;

    private void Start()
    {
        Character character = entity.GetComponent<Character>();
        character.OnSelect += Character_OnSelect;
        gameObject.SetActive(false);
    }

    private void Character_OnSelect(bool selected)
    {
        Debug.Log(selected);
        gameObject.SetActive(selected);
    }
}
