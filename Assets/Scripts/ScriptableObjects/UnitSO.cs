using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName ="ScriptableObjects/Unit")] 
public class UnitSO : ScriptableObject
{
    public Transform prefab;
    public Sprite icon;
    public string unitName;
    public float maxHealth;
    public Team team;
    public float interactionRadius;
    public float attackPower;
}
