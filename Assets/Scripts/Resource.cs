using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Resource : MonoBehaviour
{
    [SerializeField] private TextMeshPro currencyText; // Changed TextMeshPro to TextMeshProUGUI

    // Singleton pattern is used because only one instance of the wallet is made in a save file
    public static Resource Instance; // Changed Resources to Instance for clarity

    // Dictionary for better and more compact storage using enum for fixed types
    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold, 0 },
        { ResourceType.Wood, 0 },
        { ResourceType.Metal, 0 }
    };

    // Enum for fixed types where materials acquired by the player are (Currency, Wood, Metal)
    public enum ResourceType
    {
        Gold, Wood, Metal
    }

    // Using Awake to ensure resources are displayed at the start
    // Update is not necessary here, as the resources are updated whenever their values change

    private void Awake()
    {
        Instance = this; // Assigning the current instance to the singleton
        UpdateResourceText();
    }

    // If the dictionary (resources) contains the resource type, return the value; if not, return 0
    public int GetResource(ResourceType resourceType)
    {
        return resources.ContainsKey(resourceType) ? resources[resourceType] : 0;
    }

    // Method to update the resource amount
    public void UpdateResource(int amount, ResourceType resourceType)
    {
        resources[resourceType] += amount;
        UpdateResourceText();
    }

    // Update the text displaying the resources
    private void UpdateResourceText()
    {
        string text = "";
        foreach (ResourceType resourceType in resources.Keys)
        {
            int amount = GetResource(resourceType);

            // Use Unity's Rich Text <sprite> tag to include the sprite in the text
            text += $"<sprite name={resourceType}> {resourceType}: {amount}\n";
        }
        currencyText.text = text;
    }
}
