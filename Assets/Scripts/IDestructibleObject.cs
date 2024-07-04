using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface to indicate the objects that can be destroyed
public interface IDestructibleObject 
{
    public float HealthPoints { set; get; }

    public event Action <float>OnDamaged;
    public void Damage(Vector3 position,float value);
    public void Destruct();
}