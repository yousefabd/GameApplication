using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Scriptable Objects/CharacterSO")] 
public class CharacterSO : ScriptableObject
{
    public Transform prefab;
    public Sprite icon;
    public string characterName;
}
