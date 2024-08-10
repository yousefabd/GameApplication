using Assets.HeroEditor4D.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    [SerializeField] GameObject BuildingContent;
    [SerializeField] GameObject UnitContent;

    [SerializeField] Transform contentParent;

    private bool unitToggle = false;
    private GameObject unitContent;
    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
        if (!unitToggle)
        {
            BuildingContent.SetActive(true);
        }
    }

    private void Update()
    {
        checkIfMousePressed();
    }

    public void SwitchContent(bool setToggle)
    {
        unitToggle = setToggle;
        if (unitToggle)
        {
            BuildingContent.SetActive(false);
           unitContent = Instantiate(UnitContent, contentParent);
           unitContent.SetActive(true);
        }
        else if (!unitToggle)
        {
            BuildingContent.SetActive(true);
            Destroy(unitContent.gameObject);
            
            
        }
    }

    public void checkIfMousePressed()
    {
        if (Input.GetMouseButtonDown(0))
        {

            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {

                return;
            }
            GridManager.Instance.WorldToGridPosition(BuildingManager.Instance.GetMouseWorldPosition(), out int i, out int j);
            Indices indices = new Indices(i, j);
            Collider2D[] colliders = GridManager.Instance.OverlapAll(indices);

            if (colliders.Length == 0 || colliders[0].gameObject.GetComponent<Building>() == null)
            {
                SwitchContent(false);
            }
        }
    }
}