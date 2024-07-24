using CodeMonkey.Utils;
using System;
using UnityEngine;

public class ScreenInteractionManager : MonoBehaviour
{
    private Vector3 startPosition;
    [SerializeField] private Transform selectionAreaTransform;
    public static ScreenInteractionManager Instance { get; private set; }
    public event Action<Vector3> OnRightMouseButtonClicked;
    public event Action<Vector3, Vector3> OnAreaSelected;
    public event Action<Entity> OnEntityHovered;
    private void Awake()
    {
        Instance = this;
        selectionAreaTransform.gameObject.SetActive(false);
    }
    private void Update()
    {
        //debug
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        OnEntityHovered?.Invoke(GridManager.Instance.GetEntity(mousePosition));
        if (Input.GetMouseButtonDown(0))
        {
            //left mouse button pressed
            //gridMap.GetValue(mousePosition).SetEntity(Entity.OBSTACLE);
            startPosition = UtilsClass.GetMouseWorldPosition();
            selectionAreaTransform.gameObject.SetActive(true);
        }
        if (Input.GetMouseButton(0))
        {
            //while left mouse button pressed
            Vector3 currentMousePosition = UtilsClass.GetMouseWorldPosition();
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(startPosition.x, currentMousePosition.x),
                Mathf.Min(startPosition.y, currentMousePosition.y)
            );
            Vector3 upperRight = new Vector3(
                Mathf.Max(startPosition.x, currentMousePosition.x),
                Mathf.Max(startPosition.y, currentMousePosition.y)
            );
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }
        if (Input.GetMouseButtonUp(0))
        {
            //left mouse button released
            selectionAreaTransform.gameObject.SetActive(false);
            OnAreaSelected?.Invoke(startPosition, mousePosition);
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnRightMouseButtonClicked?.Invoke(mousePosition);
        }
    }
}