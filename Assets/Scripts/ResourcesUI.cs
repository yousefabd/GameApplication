using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour
{
    private ResourceTypeListSO resourceTypeList;
    private Dictionary<ResourceTypeSO, Transform> resourceTypeTransfomDictionary;

    private void Awake()
    {
        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
       
        resourceTypeTransfomDictionary = new Dictionary<ResourceTypeSO, Transform>();

        Transform resourceTemplate = transform.Find("resourceTemplate");
        resourceTemplate.gameObject.SetActive(false);

        
         int index = 0;

        foreach (ResourceTypeSO resourceType in resourceTypeList.list) {

            Transform resourceTransform = Instantiate(resourceTemplate,transform);
            resourceTransform.gameObject.SetActive(true);

            float offsetAmount = -160f;
            resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(index * offsetAmount, 0);

            resourceTransform.Find("image").GetComponent<Image>().sprite = resourceType.sprite;
            resourceTypeTransfomDictionary[resourceType] = resourceTransform;
            index++;

        }
    }
    private void Start()
    {
        ResourceManager.instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
        UpdateResourceAmount();
    }

    private void ResourceManager_OnResourceAmountChanged(object sender, System.EventArgs e)
    {
     UpdateResourceAmount();
    }
    
    private void UpdateResourceAmount() {
        foreach (ResourceTypeSO resourceType in resourceTypeList.list) {
            Transform resourceTransform = resourceTypeTransfomDictionary[resourceType];

            int resourceAmount = ResourceManager.instance.GetResourceAmount(resourceType);

            resourceTransform.Find("text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
        }   
    }
}

