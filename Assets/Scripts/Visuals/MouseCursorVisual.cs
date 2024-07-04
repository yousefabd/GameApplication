using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseCursorVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer mouseSprite;
    [SerializeField] private List<MouseCursorSO> mouseCursors;
    private Dictionary<string, Sprite> mouseCursorsDictionary;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        mouseCursorsDictionary= new Dictionary<string, Sprite>();
        ScreenInteractionManager.Instance.OnEntityHovered += ScreenInteractionManager_OnEntityHovered;
        foreach (MouseCursorSO mouseCursorSO in mouseCursors) 
        {
            mouseCursorsDictionary[mouseCursorSO.entityName] = mouseCursorSO.cursorSprite;
        }
    }

    private void ScreenInteractionManager_OnEntityHovered(Entity entity)
    {
        if (entity == null)
        {
            mouseSprite.sprite = GetMouseSprite("Null");
        }
        else
        {
            mouseSprite.sprite = GetMouseSprite(entity.GetType().ToString());
        }
    }
    private Sprite GetMouseSprite(string entityName)
    {
        if (mouseCursorsDictionary.ContainsKey(entityName))
        { 
            return mouseCursorsDictionary[entityName];
        }
        return mouseCursorsDictionary["Null"];
    }
    // Update is called once per frame
    private void Update()
    {
        transform.position = UtilsClass.GetMouseWorldPosition();
    }
}
