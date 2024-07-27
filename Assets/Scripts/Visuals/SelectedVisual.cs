using UnityEngine;

public class SelectedVisual : MonoBehaviour
{
    [SerializeField] private Transform entity;
    private void Awake()
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

