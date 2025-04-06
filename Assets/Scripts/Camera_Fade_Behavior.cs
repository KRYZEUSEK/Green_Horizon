using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Input = UnityEngine.Windows.Input;

public class Camera_Fade_Behavior : MonoBehaviour
{
    public CinemachineVirtualCamera virtual_camera;
    public CinemachineVirtualCamera next_camera;
    public float timer;
    public CinemachineDollyCart dolly_cart;
    void OnEnable()
    {
        timer = 2.0f;
    }

    private void OnDisable()
    {
        dolly_cart.m_Speed = 0.0f;
        dolly_cart.m_Position = 0;
    }

    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
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
    }
}
