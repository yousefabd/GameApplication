using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointPath : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPointsList;
    [SerializeField] private Vector2 spawnOffset;
    System.Random random;
    public static WayPointPath Instance { get; private set; }
    private void Awake()
    {
        random = new System.Random();
        Instance = this;
    }

    private void Start()
    {
        HidePaths();
    }
    private void HidePaths()
    {
        for(int i=0;i<spawnPointsList.Count; i++)
        {
            foreach(Transform path in spawnPointsList[i])
            {
                path.gameObject.SetActive(false);
            }
            spawnPointsList[i].gameObject.SetActive(false);
        }
    }
    public Vector3 GetRandomPath(out List<Vector3> path)
    {
        float xOffset = (float)(random.NextDouble()*2*spawnOffset.x)-spawnOffset.x;
        float yOffset = (float)(random.NextDouble() * 2 * spawnOffset.y) - spawnOffset.y;
        Vector3 offset = new Vector3(xOffset, yOffset, 0);
        Vector3 spawnPointPosition;
        path = new List<Vector3>();
        int randomIndex=random.Next(spawnPointsList.Count);
        spawnPointPosition=spawnPointsList[randomIndex].position;
        foreach (Transform pathNode in spawnPointsList[randomIndex])
        {
            path.Add(pathNode.position+offset);
        }
        return spawnPointPosition+offset;
    }
}
