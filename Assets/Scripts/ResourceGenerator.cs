    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private float timer;
    private float timerMax;
    private BuildingSO buildingSO;
    private Resource resourceManager = Resource.Instance;

    private void Awake()
    {
        buildingSO = GetComponent<Building>().buildingSO;
        timerMax = buildingSO.resourceGeneratorData.TimerMax;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer += timerMax;
            resourceManager.UpdateResource(1,buildingSO.resourceGeneratorData.ResourceType);
        }
    }
}
