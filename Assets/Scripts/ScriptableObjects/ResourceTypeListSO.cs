using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ResourceTypeListSO")]
public class ResourceTypeListSO : ScriptableObject
{
  public List<ResourceTypeSO> list;
}
