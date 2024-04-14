/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit {
    
    Vector3 GetPosition();
    void SetPosition(Vector3 position);

    int GetGoldAmount();
    void SetGoldAmount(int goldAmount);

}
