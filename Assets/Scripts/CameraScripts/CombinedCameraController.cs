using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class CombinedCameraController : MonoBehaviour
{
    public CameraManager cameraManager;
    public OrbitingCameraScript orbitingCameraScript;
    public IEnumerator cameraCycle;
    
    private bool isOrbitingActive = false;

    private void Start()
    {
        cameraCycle = cameraManager.CameraCycleCoroutine();
    }

    private void Update()
    {
        if (orbitingCameraScript.orbitingTarget.activeSelf &&
            !isOrbitingActive)
        {
            // Pause camera cycling by stopping its coroutine
            StopCoroutine(cameraCycle);

            // Start orbiting behavior
            isOrbitingActive = true;
            StartCoroutine(StartOrbitThenResume());
        }
    }
    private IEnumerator StartOrbitThenResume()
    {
        // Manually invoke the orbiting behavior
        yield return StartCoroutine(orbitingCameraScript.WaitForTargetAndOrbit());

        // Resume camera cycling after orbit ends
        StartCoroutine(cameraCycle);
        isOrbitingActive = false;
    }
}