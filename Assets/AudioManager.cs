using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audios;
    [SerializeField] private AudioSource currentAudio;

    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        BuildingManager.Instance.built += Instance_built;
    }

    private void Instance_built(Building obj)
    {
        currentAudio.PlayOneShot(audios[0]);
    }
}
