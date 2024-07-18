using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject buildingContent,unitContent;

    private bool scrollToggle=false;

    private void Start()
    {
        if (!scrollToggle)
        {
            buildingContent.SetActive(true);
            unitContent.SetActive(false);
        }
    }
    private void Update()
    {
        checkIfMousepressed();
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
    public void checkIfMousepressed()
    {
        if(Input.GetMouseButton(0))
        {
            SwitchContent(false);
        }
    }









}
