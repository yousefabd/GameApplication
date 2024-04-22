using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Entity
{
    SAFE,OBSTACLE,BUILDING,CHARACTER
}
public class Cell
{
    public Entity entity = Entity.SAFE;
}
