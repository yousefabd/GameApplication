using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public virtual Entity Spawn(Cell cell,out bool Initiated) {
        Initiated = false;
        Entity entity = null;
        return  entity;

    }
}
