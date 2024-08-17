using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameMenu : MonoBehaviour
{
    [SerializeField] private Button Button;
    [SerializeField] private Button snowButton;


    private void Awake()
    {
        Button.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.test);
        });
        snowButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.SnowScene);
        });

        Time.timeScale = 1f;
    }
}
