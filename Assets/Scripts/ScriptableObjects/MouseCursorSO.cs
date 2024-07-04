using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/MouseCursor")]
public class MouseCursorSO : ScriptableObject
{
    public string entityName;
    public Sprite cursorSprite;
}
