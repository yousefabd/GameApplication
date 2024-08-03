using UnityEngine;
[CreateAssetMenu(menuName ="ScriptableObjects/Unit")] 

public class UnitSO : ScriptableObject
{
    public Transform prefab;
    public Sprite icon;
    public string unitName;
    public float maxHealth;
    public float moveSpeed;
    public Team team;
    public float interactionRadius;
    public float attackPower;
    public Unit unit { get { return prefab.GetComponent<Unit>(); } }
    public float attackCooldown;
    public SoldierType soldierType;

}
