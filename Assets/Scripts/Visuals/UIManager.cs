using Assets.HeroEditor4D.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject BuildingContent;
    [SerializeField] GameObject UnitContent;

    private bool scrollToggle = false;

    public static UIManager Instance;

    private GameObject buildingContent;
    private GameObject unitContent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Initialize the buildingContent and unitContent
        buildingContent = BuildingContent;
        unitContent = UnitContent;

        if (!scrollToggle)
        {
            buildingContent.SetActive(true);
            unitContent.SetActive(false);
        }
    }

    private void Update()
    {
        checkIfMousePressed();
    }

    public void SwitchContent(bool setToggle)
    {
        scrollToggle = setToggle;
        if (scrollToggle)
        {
            buildingContent.SetActive(false);
            unitContent.SetActive(true);
        }
        else
        {
            buildingContent.SetActive(true);
            unitContent.SetActive(false);
        }
    }

    public void checkIfMousePressed()
    {
        Debug.Log("wtf");
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse pressed");


            GridManager.Instance.WorldToGridPosition(BuildingManager.Instance.GetMouseWorldPosition(), out int i, out int j);
            Indices indices = new Indices(i,j);
            Collider2D[] colliders = GridManager.Instance.OverlapAll(indices);

            foreach (Collider2D collider in colliders)
            {
                Debug.Log(collider);
            }

            if (colliders.Length == 0 || colliders[0].gameObject.GetComponent<Building>() == null)
            {
                SwitchContent(false);
            }


        }
    }
}