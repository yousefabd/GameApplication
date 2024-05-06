using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public virtual void Spawn(Cell cell,out bool Initiated) {
        Initiated = false;
    }
}
