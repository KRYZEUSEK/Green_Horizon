using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Camera_Behavior : MonoBehaviour
{
    public CinemachineVirtualCamera virtual_camera;
    public CinemachineVirtualCamera next_camera;
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        {
            Change_Camera();
        }
    }
    private void Change_Camera()
    {
        if (virtual_camera.isActiveAndEnabled)
        {
            virtual_camera.gameObject.SetActive(false);
            next_camera.gameObject.SetActive(true);
        }
        else if (next_camera.isActiveAndEnabled)
        {
            next_camera.gameObject.SetActive(false);
            virtual_camera.gameObject.SetActive(true);
        }
    }
}
