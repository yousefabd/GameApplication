using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] private Transform mouseTransform;
    [SerializeField] private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mouseTransform.position = GetMouseWorldPosition();
    }

    // Update is called once per frame
    void Update()
    {
        mouseTransform.position = GetMouseWorldPosition();
    }
    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = mainCamera.nearClipPlane;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }
}
