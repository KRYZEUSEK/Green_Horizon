using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Camera_Menager : MonoBehaviour
{
    public CinemachineVirtualCamera[] virtual_cameras;
    public CinemachineVirtualCamera virtual_camera_fade;
    private int current_camera_index = 0;
    public CinemachineDollyCart[] dolly_carts;
    void Start()
    {
        StartCoroutine(Cycle_Coroutine());
        ActivateDolly(current_camera_index);
    }
    private void ActivateDolly(int index)
    {
        dolly_carts[index].m_Speed = 0.05f;
    }
    
    private void DeactivateDolly(int index)
    {
        dolly_carts[index].m_Speed = 0.0f;
        dolly_carts[index].m_Position = 0;
    }
    private void SwitchToNextCamera(CinemachineVirtualCamera virtualCameraFade, CinemachineVirtualCamera virtualCamera, int index)
    {
        virtualCameraFade.gameObject.SetActive(false);
        virtualCamera.gameObject.SetActive(true);
        ActivateDolly(index);
    }
    private void SwitchToBlack(CinemachineVirtualCamera virtualCamera, CinemachineVirtualCamera virtualCameraFade, int index)
    {
        virtualCamera.gameObject.SetActive(false);
        virtualCameraFade.gameObject.SetActive(true);
        DeactivateDolly(index);
    }

    private IEnumerator Cycle_Coroutine()
    {
        for (int i = 0; i <= virtual_cameras.Length; i++)
        {
            if (i > virtual_cameras.Length)
            {
                i = 0;
            }
            yield return new WaitForSeconds(8);
            SwitchToBlack(virtual_cameras[i], virtual_camera_fade, i);
            yield return new WaitForSeconds(2);
            SwitchToNextCamera(virtual_camera_fade, virtual_cameras[i + 1], i + 1);
        }
    }
}
