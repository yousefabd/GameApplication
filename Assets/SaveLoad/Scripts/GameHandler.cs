/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour {

    [SerializeField] private GameObject unitGameObject;
    private IUnit unit;

    private void Awake() {
        unit = unitGameObject.GetComponent<IUnit>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            Load();
        }
    }

    private void Save() {
        // Save
        Vector3 playerPosition = unit.GetPosition();
        int goldAmount = unit.GetGoldAmount();

        PlayerPrefs.SetFloat("playerPositionX", playerPosition.x);
        PlayerPrefs.SetFloat("playerPositionY", playerPosition.y);

        PlayerPrefs.SetInt("goldAmount", goldAmount);

        PlayerPrefs.Save();

        CMDebug.TextPopupMouse("Saved!");
    }

    private void Load() {
        // Load
        if (PlayerPrefs.HasKey("playerPositionX")) {
            float playerPositionX = PlayerPrefs.GetFloat("playerPositionX");
            float playerPositionY = PlayerPrefs.GetFloat("playerPositionY");
            Vector3 playerPosition = new Vector3(playerPositionX, playerPositionY);
            int goldAmount = PlayerPrefs.GetInt("goldAmount", 0);
            CMDebug.TextPopupMouse("Loaded!");

            unit.SetPosition(playerPosition);
            unit.SetGoldAmount(goldAmount);
        } else {
            // No save is available
            CMDebug.TextPopupMouse("No save");
        }
    }

}
