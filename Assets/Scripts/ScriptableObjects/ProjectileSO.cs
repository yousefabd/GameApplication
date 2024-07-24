using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[CreateAssetMenu(menuName = "ScriptableObjects/Projectile")]
public class ProjectileSO : ScriptableObject
{
    public Transform prefab;
    public float damage;
    public float speed;
    public float effectRadius;
    public string projectileName;
    public ProjectileType projectileType;
}
