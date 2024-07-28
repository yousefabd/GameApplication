/* 
    ------------------- Code Monkey -------------------
    
    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using UnityEngine;

namespace CodeMonkey.MonoBehaviours
{
    /*
     * Easy set up for CameraFollow, it will follow the mouse position with zoom
     * */
    public class CameraFollowSetup : MonoBehaviour
    {
        [SerializeField] private CameraFollow cameraFollow;
        [SerializeField] private float zoom;

        private void Start()
        {
            cameraFollow.Setup(() => GetMouseWorldPosition(), () => 1);
        }

        private void Update()
        {
        }

        private Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            return Camera.main.ScreenPointToRay(mousePos).origin;
        }
    }
}