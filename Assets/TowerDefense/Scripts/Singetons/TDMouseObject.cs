using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDMouseObject : MonoBehaviour
{
    public static TDMouseObject Instance { get; private set; }
    private bool hasActive = false;

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleActive(Transform obj)
    {
        if (obj.gameObject.activeSelf)
        {
            obj.gameObject.SetActive(false);
            hasActive = false;
        }
        else if (!hasActive)
        {
            obj.gameObject.SetActive(true);
            hasActive = true;
        }
    }

}
