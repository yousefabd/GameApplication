using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingHealthBarVisual : MonoBehaviour
{
    private IDestructibleObject destructibleObject;
    [SerializeField] private GameObject entityGameObject;
    [SerializeField] Image health;

    private float maxHealth;
    private float currentHealth;

    private void Start()
    {
        // Get the IDestructibleObject component from the entityGameObject
        destructibleObject = entityGameObject.GetComponent<IDestructibleObject>();

        if (destructibleObject == null)
        {
            Debug.LogError("The entity GameObject " + entityGameObject + " does not have an IDestructibleObject component");
        }
        else
        {
            maxHealth = destructibleObject.HealthPoints;
            currentHealth = maxHealth;
            destructibleObject.OnDamaged += DestructibleObject_OnDamaged;
        }

        UpdateVisual();
    }

    private void DestructibleObject_OnDamaged(float damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        Debug.Log(health.fillAmount);


        health.fillAmount = currentHealth / maxHealth;
        Debug.Log(health.fillAmount);

    }
}