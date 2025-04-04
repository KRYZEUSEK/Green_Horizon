using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Camera_Dolly_Behavior : MonoBehaviour
{
    public CinemachineVirtualCamera virtual_camera;
    public CinemachineVirtualCamera fade_camera;
    public CinemachineVirtualCamera next_camera;
    public float timer = 5.0f;
    void FixedUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            Fade_To_Black();
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

    private void Fade_To_Black()
    {
        if (!fade_camera.isActiveAndEnabled)
        {
            fade_camera.gameObject.SetActive(true);
            virtual_camera.gameObject.SetActive(false);
        }
    }
}
