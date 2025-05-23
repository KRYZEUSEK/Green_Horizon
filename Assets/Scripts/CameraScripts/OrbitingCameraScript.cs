using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class OrbitingCameraScript : MonoBehaviour
{
    [Header("Camera Settings")]
    public CinemachineVirtualCamera orbitingCamera;
    public float orbitSpeed = 30f; // Degrees per second
    public float orbitDuration = 6f;

    [Header("Target Settings")]
    public GameObject orbitingTarget;
    public Vector3 cameraOffset = new Vector3(0, 5f, -10f); // Default offset if none given

    private CinemachineOrbitalTransposer orbital;
    private float currentAngle = 0f;
    private float orbitTimer = 0f;
    private bool isOrbiting = false;

    private void Start()
    {
        orbital = orbitingCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        StartCoroutine(WaitForTargetAndOrbit());
    }

    public IEnumerator WaitForTargetAndOrbit()
    {
        if (orbitingTarget.activeSelf)
        {
            FocusOnNewTarget(orbitingTarget.transform);

            orbitingCamera.gameObject.SetActive(true);
            isOrbiting = true;
            orbitTimer = orbitDuration;

            while (orbitTimer > 0f)
            {
                Orbit();
                orbitTimer -= Time.deltaTime;
                yield return null;
            }

            orbitingCamera.gameObject.SetActive(false);
            isOrbiting = false;
        }
    }

    public void Orbit()
    {
        if (!isOrbiting || orbital == null) return;

        currentAngle += orbitSpeed * Time.deltaTime;
        currentAngle %= 360f;

        orbital.m_Heading.m_Bias = currentAngle;
    }

    public void FocusOnNewTarget(Transform newTarget)
    {
        if (orbitingCamera == null || orbital == null) return;

        orbitingCamera.Follow = newTarget;
        orbitingCamera.LookAt = newTarget;

        orbital.m_FollowOffset = cameraOffset;
        currentAngle = orbital.m_Heading.m_Bias;
    }
}
