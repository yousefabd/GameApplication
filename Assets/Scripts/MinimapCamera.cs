using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform followingTarget;

    private void LateUpdate()
    {
        Vector3 newPositions = followingTarget.position;
        newPositions.z = -20;
        transform.position = newPositions;
        transform.rotation = Quaternion.Euler(0,90f,0);
    }


}
