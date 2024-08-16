using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Building")]
public class BuildingSO : ScriptableObject
{
    public string stringName;
    public Transform buildingPrefab;
    public float health;
    public Sprite buildingSprite;
    public int price;
    public int wood;
    public int stone;


   public BuildingType buildingType;
    public ResourceType resourceType;



    public List<UnitSO> unitGenerationData = new List<UnitSO>();




    public Building building { get { return buildingPrefab.GetComponent<Building>(); } }

    }

