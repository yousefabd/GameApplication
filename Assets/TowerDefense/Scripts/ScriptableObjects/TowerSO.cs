using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/TowerDefense/Tower")]
public class TowerSO : ScriptableObject
{
    public Transform prefab;
    public string towerName;
    public int cost;
}
