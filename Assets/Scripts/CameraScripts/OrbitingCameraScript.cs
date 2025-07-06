using System.Collections;
using Cinemachine;
using UnityEngine;

public class OrbitingCameraScript : MonoBehaviour
{
    [Header("Camera Settings")]
    public CinemachineVirtualCamera orbitingCamera;
    public bool shouldOrbit = true;
    public float orbitSpeed = 30f; // Degrees per second
    public float minAngle = 0f; // Minimum angle for orbiting
    public float maxAngle = 360f; // Maximum angle for orbiting
    public bool waitForDecision = false; // Orbit until a decision is made
    public float orbitDuration = 6f; // Duration to orbit before stopping, if not waiting for a decision

    [Header("Target Settings")]
    public GameObject orbitingTarget;
    public Vector3 cameraOffset = new Vector3(0, 5f, -10f); // Default offset if none given

    private CameraManager cameraManager;
    private CinemachineOrbitalTransposer orbital;
    private float currentAngle = 0f;
    private bool isRotatingInOtherDirection = false;
    private float orbitTimer = 0f;

    private bool isOrbiting = false;
    private bool wasDecisionChosen = false;

    private void OnEnable()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        orbital = orbitingCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        GameManager.Instance.onDecisionChoice.AddListener(StopOrbiting);

        DisruptCameraCycle();
        StartCoroutine(WaitForTargetAndOrbit());
    }

    private void StopOrbiting() {
        wasDecisionChosen = true;
    }

    private IEnumerator WaitForTargetAndOrbit()
    {
        FocusOnNewTarget(orbitingTarget.transform);

        orbitingCamera.gameObject.SetActive(true);
        DisruptCameraCycle();
        isOrbiting = shouldOrbit;
        orbitTimer = orbitDuration;

        isRotatingInOtherDirection = false;
        currentAngle = minAngle;
        orbital.m_Heading.m_Bias = currentAngle;

        while (CanOrbit())
        {
            Orbit();
            orbitTimer -= Time.deltaTime;
            yield return null;
        }

        isOrbiting = false;
        orbitingCamera.gameObject.SetActive(false);
        ContinueCameraCycle();
        this.enabled = false;
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

    private void Orbit()
    {
        if (!isOrbiting || orbital == null) return;

        if (isRotatingInOtherDirection) {
            currentAngle -= orbitSpeed * Time.deltaTime;
        }
        else {
            currentAngle += orbitSpeed * Time.deltaTime;
        }
        
        currentAngle %= 360f;

        if (currentAngle <= minAngle || currentAngle >= maxAngle) {
            isRotatingInOtherDirection = !isRotatingInOtherDirection;
        }

        orbital.m_Heading.m_Bias = currentAngle;
    }

    private void FocusOnNewTarget(Transform newTarget)
    {
        if (orbitingCamera == null || orbital == null) return;

        orbitingCamera.Follow = newTarget;
        orbitingCamera.LookAt = newTarget;

        orbital.m_FollowOffset = cameraOffset;
        currentAngle = orbital.m_Heading.m_Bias;
    }
}
