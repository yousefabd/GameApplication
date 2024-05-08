using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/BuildingSO")]
public class BuildingSO : ScriptableObject 
{
    public string stringName;
    public Transform buildingPrefab;
    public int width,height;
    public Building building { get { return buildingPrefab.GetComponent<Building>(); } }
    public ResourceGeneratorData resourceGeneratorData; 
}
