using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private Vector3 startPosition;
    [SerializeField] private Transform selectionAreaTransform;
    public static MouseManager Instance { get; private set; }
    public event Action<Vector3> OnWalk;

    private void Awake()
    {
        Instance = this;
        selectionAreaTransform.gameObject.SetActive(false);
    }
    private void Start()
    {
    }
    private void Update()
    {
        //debug
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        if (Input.GetMouseButtonDown(0))
        {
            //left mouse button pressed
            //gridMap.GetValue(mousePosition).SetEntity(Entity.OBSTACLE);
            startPosition=UtilsClass.GetMouseWorldPosition();
            selectionAreaTransform.gameObject.SetActive(true);
        }
        if (Input.GetMouseButton(0))
        {
            //while left mouse button pressed
            Vector3 currentMousePosition = UtilsClass.GetMouseWorldPosition();
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(startPosition.x,currentMousePosition.x),
                Mathf.Min(startPosition.y,currentMousePosition.y)
            );
            Vector3 upperRight = new Vector3(
                Mathf.Max(startPosition.x,currentMousePosition.x),
                Mathf.Max(startPosition.y,currentMousePosition.y)
            );
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }
        if (Input.GetMouseButtonUp(0))
        {
            //left mouse button released
            selectionAreaTransform.gameObject.SetActive(false);
            Player.Instance.ClearSelectedCharacters();
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition,UtilsClass.GetMouseWorldPosition());
            foreach (Collider2D collider2D in collider2DArray)
            {
                Unit character = collider2D.GetComponent<Unit>();
                Player.Instance.AddSelectedCharacter(character);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnWalk?.Invoke(mousePosition);
        }
    }
}