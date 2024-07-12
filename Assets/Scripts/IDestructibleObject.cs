using System;
using UnityEngine;

//Interface to indicate the objects that can be destroyed
public interface IDestructibleObject
{
    public float HealthPoints { get; set; }

    public event Action <float>OnDamaged;

    public event Action OnDestroyed;
    public void Damage(Vector3 position,float value);
    public void Destruct();
}