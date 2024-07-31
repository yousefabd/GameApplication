using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellButtonUI : MonoBehaviour
{
    private Button sellButton;
    [SerializeField] private Transform sellBagTransform;
    bool selling = false;
    public event Action OnClickBag;
    public event Action<Vector3> OnSell;
    public static SellButtonUI Instance { get; private set; }
    private void Awake()
    {
        sellButton = GetComponent<Button>();
        Instance = this;
    }
    private void Start()
    {
        sellButton.onClick.AddListener(() =>
        {
            OnClickBag?.Invoke();
        });
    }
    public void Sell(Vector3 position)
    {
        OnSell?.Invoke(position);
    }
}
