using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBarTextUI : MonoBehaviour
{
    private TextMeshProUGUI healthText;
    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        string maxHeatlh = ((int)TDCastle.Instance.GetMaxHealth()).ToString();
        string currentHealth = ((int)TDCastle.Instance.HealthPoints).ToString();
        healthText.text = currentHealth + " / " + maxHeatlh;
    }
}
