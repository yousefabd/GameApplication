using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuildingView : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        BuildingManager.Instance.built += Instance_built;
    }

    private void Instance_built(Building obj)
    {
        if(obj.buildingSO.buildingType == BuildingType.BaseBuilding)
        {
            virtualCamera.transform.position = obj.transform.position;
        }
    }
}
