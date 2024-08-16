using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/TowerDefense/Audio")]
public class AudioRefsSO : ScriptableObject
{
    public AudioClip[] countdown;
    public AudioClip[] goblinDeaths;
    public AudioClip buy;
}
