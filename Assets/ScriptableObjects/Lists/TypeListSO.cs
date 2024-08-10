using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/BuildingSOList")]
public class BuildingSOList : ScriptableObject
{
    public List<BuildingSO> buildingSOList = new List<BuildingSO>();
}
