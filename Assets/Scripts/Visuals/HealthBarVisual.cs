using UnityEngine;
using UnityEngine.UI;

public class HealthBarVisual : MonoBehaviour
{
    private IDestructibleObject destructibleObject;
    [SerializeField] private Entity entity;
    [SerializeField] Image health;

    private float maxHealth;
    private float currentHealth;
    private void Start()
    {
        destructibleObject = entity.GetComponent<IDestructibleObject>();
        if (destructibleObject == null)
        {
            Debug.LogError("this entity " + entity + " is not destructible");
        }
        else
        {
            maxHealth = destructibleObject.HealthPoints;
            currentHealth = maxHealth;
            destructibleObject.OnDamaged += DestructibleObject_OnDamaged;
            if(destructibleObject is Unit)
            {
                (destructibleObject as Unit).OnMaxHealthChanged += HealthBarVisual_OnMaxHealthChanged;
            }
        }
    }

    private void HealthBarVisual_OnMaxHealthChanged(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = newMaxHealth;
        UpdateVisual();
    }

    private void DestructibleObject_OnDamaged(float value)
    {
        currentHealth -= value;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        health.fillAmount = (currentHealth / maxHealth);
    }
}
