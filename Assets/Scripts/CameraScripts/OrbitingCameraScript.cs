using System.Collections;
using Cinemachine;
using UnityEngine;

public class OrbitingCameraScript : MonoBehaviour
{
    [Header("Camera Settings")]
    public CinemachineVirtualCamera orbitingCamera;
    public bool shouldOrbit = true;
    public float orbitSpeed = 30f; // Degrees per second
    public bool waitForDecision = false; // Orbit until a decision is made
    public float orbitDuration = 6f; // Duration to orbit before stopping, if not waiting for a decision

    [Header("Target Settings")]
    public GameObject orbitingTarget;
    public Vector3 cameraOffset = new Vector3(0, 5f, -10f); // Default offset if none given

    private CameraManager cameraManager;
    private CinemachineOrbitalTransposer orbital;
    private float currentAngle = 0f;
    private float orbitTimer = 0f;

    private bool isOrbiting = false;
    private bool wasDecisionChosen = false;

    private void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        orbital = orbitingCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        StartCoroutine(WaitForTargetAndOrbit());
        GameManager.Instance.onDecisionChoice.AddListener(StopOrbiting);
    }

    private void StopOrbiting() {
        wasDecisionChosen = true;
    }

    public IEnumerator WaitForTargetAndOrbit()
    {
        if (orbitingTarget.activeSelf)
        {
            FocusOnNewTarget(orbitingTarget.transform);

            orbitingCamera.gameObject.SetActive(true);
            DisruptCameraCycle();
            isOrbiting = shouldOrbit;
            orbitTimer = orbitDuration;

            while (CanOrbit())
            {
                Orbit();
                orbitTimer -= Time.deltaTime;
                yield return null;
            }

            ContinueCameraCycle();
            orbitingCamera.gameObject.SetActive(false);
            isOrbiting = false;
        }
    }

    private bool CanOrbit() {
        if (waitForDecision) {
            return wasDecisionChosen == false;
        }

        return orbitTimer > 0f;
    }

    private void DisruptCameraCycle() {
        cameraManager.DisruptCameraCycle();
    }

    private void ContinueCameraCycle() {
        cameraManager.ContinueCameraCycle();
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
