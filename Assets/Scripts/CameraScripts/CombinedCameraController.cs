using System.Collections;
using Cinemachine;
using UnityEngine;

public class CombinedCameraController : MonoBehaviour
{
    public CameraManager cameraManager;
    public OrbitingCameraScript orbitingCameraScript;

    private Coroutine monitorCoroutine;
    private bool isOrbitingActive = false;

    private void Start()
    {
        if (cameraManager != null && orbitingCameraScript != null)
        {
            monitorCoroutine = StartCoroutine(ManageCameraCycleWithOrbit());
        }
    }

    private IEnumerator ManageCameraCycleWithOrbit()
    {
        while (true)
        {
            if (orbitingCameraScript.orbitingTarget != null &&
                orbitingCameraScript.orbitingTarget.activeInHierarchy &&
                !isOrbitingActive)
            {
                // Pause camera cycling by stopping its coroutine
                StopCoroutine(cameraManager.StartCoroutine("CameraCycleCoroutine"));

                // Start orbiting behavior
                isOrbitingActive = true;
                yield return StartCoroutine(StartOrbitThenResume());
            }

            yield return null;
        }
    }

    private IEnumerator StartOrbitThenResume()
    {
        // Manually invoke the orbiting behavior
        yield return StartCoroutine(orbitingCameraScriptWaiter());

        // Resume camera cycling after orbit ends
        cameraManager.StartCoroutine("CameraCycleCoroutine");
        isOrbitingActive = false;
    }

    private IEnumerator orbitingCameraScriptWaiter()
    {
        // Recreate logic from OrbitingCameraScript's coroutine
        orbitingCameraScript.FocusOnNewTarget(orbitingCameraScript.orbitingTarget.transform);

        orbitingCameraScript.orbitingCamera.gameObject.SetActive(true);

        float orbitTimer = orbitingCameraScript.orbitDuration;
        var orbital = orbitingCameraScript.orbitingCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        while (orbitTimer > 0f)
        {
            orbitingCameraScript.Orbit(); // orbit logic runs
            orbitTimer -= Time.deltaTime;
            yield return null;
        }

        orbitingCameraScript.orbitingCamera.gameObject.SetActive(false);
    }
}