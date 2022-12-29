using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vitrin.PlayerCameraController
{
    public class PlayerCamerController : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 100)]
        float camSensivity;

        float mouseX;

        float mouseY;

        float xRotation;

        [SerializeField]
        float horizontalSpeed;

        [SerializeField]
        float Verticalspeed;

        [SerializeField]
        Transform player;

        [SerializeField]
        Transform cameraParent;

        bool isCursor;

        void LateUpdate()
        {
            mouseX = SimpleInput.GetAxis("Mouse X") * camSensivity * Time.deltaTime;
            mouseY =
                SimpleInput.GetAxis("Mouse Y") *
                camSensivity *
                Verticalspeed *
                Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);
            player.Rotate(Vector3.up, mouseX * horizontalSpeed);
            Quaternion xrot = Quaternion.Euler(xRotation, 0, 0);
            cameraParent.localRotation = xrot;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isCursor = !isCursor;
                CursorControl (isCursor);
            }
        }

        void Update()
        {
            if (isCursor)
            {
                camSensivity = 0;
            }
            else
            {
                camSensivity = 50;
            }
        }

        void CursorControl(bool cursorStatus)
        {
            if (!cursorStatus)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
