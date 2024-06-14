using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface to indicate the objects that can be destroyed
public interface IDestructableObject 
{
    public float HealthPoints { set; get; }

    public void Damage(Indices position,float value);
    public void Destruct();
}