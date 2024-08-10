using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Imagefit : MonoBehaviour
{
    private Image myImage;

    // Assign the sprite in the Inspector
    public GameObject parent;
    public Sprite sprite;

    void Start()
    {
        // Get the Image component attached to this GameObject
        myImage = parent.GetComponent<Image>();

        // Check if the sprite is assigned
        if (sprite != null)
        {
            // Assign the sprite to the Image component
            myImage.sprite = sprite;

            // Set the Image to match the sprite's native size
            myImage.SetNativeSize();
        }
        else
        {
            Debug.LogError("Sprite not assigned! Please assign a sprite in the Inspector.");
        }
    }
}
