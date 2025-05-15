using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineVirtualCamera[] virtualCameras;
    public CinemachineVirtualCamera virtualCameraFade;

    [Header("Dolly Carts")]
    public CinemachineDollyCart[] dollyCarts;

    private int currentCameraIndex = 0;

    private void Start()
    {
        for (int i = 0; i < virtualCameras.Length; i++)
            virtualCameras[i].gameObject.SetActive(false);

        virtualCameraFade.gameObject.SetActive(false);
        virtualCameras[currentCameraIndex].gameObject.SetActive(true);
        StartCoroutine(CameraCycleCoroutine());
        ActivateDolly(currentCameraIndex);
    }

    private void ActivateDolly(int index)
    {
        if (index >= 0 && index < dollyCarts.Length)
            dollyCarts[index].m_Speed = 0.05f;
    }

    private void DeactivateDolly(int index)
    {
        if (index >= 0 && index < dollyCarts.Length)
        {
            dollyCarts[index].m_Speed = 0.0f;
            dollyCarts[index].m_Position = 0f;
        }
    }

    private void SwitchToNextCamera(int newIndex)
    {
        DeactivateDolly(currentCameraIndex);

        if (currentCameraIndex >= 0 && currentCameraIndex < virtualCameras.Length)
            virtualCameras[currentCameraIndex].gameObject.SetActive(false);

        virtualCameraFade.gameObject.SetActive(false);

        currentCameraIndex = newIndex;

        if (currentCameraIndex >= 0 && currentCameraIndex < virtualCameras.Length)
        {
            virtualCameras[currentCameraIndex].gameObject.SetActive(true);
            ActivateDolly(currentCameraIndex);
        }
    }

    private void SwitchToBlack()
    {
        if (currentCameraIndex >= 0 && currentCameraIndex < virtualCameras.Length)
            virtualCameras[currentCameraIndex].gameObject.SetActive(false);

        virtualCameraFade.gameObject.SetActive(true);
    }

    public IEnumerator CameraCycleCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(8f);
            SwitchToBlack();

            yield return new WaitForSeconds(2f);

            int nextIndex = (currentCameraIndex + 1) % virtualCameras.Length;
            SwitchToNextCamera(nextIndex);
        }
    }
}