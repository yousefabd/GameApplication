using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface to indicate the objects that can be destroyed
public interface DestructableObject 
{
    int Value { get; }
    int getValue();
    void destruct();
}