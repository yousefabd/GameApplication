using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="ScriptableObjects/Character")] 
public class CharacterSO : ScriptableObject
{
    public Transform prefab;
    public Sprite icon;
    public string characterName;
}
