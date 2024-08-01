using Assets.HeroEditor4D.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetMouseButton(0))
        {
            SwitchContent(false);
        }
    }
}