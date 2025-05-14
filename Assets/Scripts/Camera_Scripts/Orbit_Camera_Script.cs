using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Orbit_Camera_Script : MonoBehaviour
{
    public CinemachineVirtualCamera orbitingCamera;
    private Transform previousTarget;
    private float currentOrbitDistance;

    private void Start()
    {
        StartCoroutine(Target_Appear);
        var orbital = orbitingCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        orbital.
    }

    public void FocusOnNewTarget(Transform newTarget)
    {
        var orbital = orbitingCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        
        if (previousTarget != null)
        {
            currentOrbitDistance = Vector3.Distance(orbitingCamera.transform.position, previousTarget.position);
        }
        
        orbitingCamera.Follow = newTarget;
        orbitingCamera.LookAt = newTarget;
        
        if (orbital != null)
        {
            orbital.m_FollowOffset = new Vector3(0, orbital.m_FollowOffset.y, -currentOrbitDistance);
        }
        
        previousTarget = newTarget;
    }

    private IEnumerator Target_Appear()
    {
        yield return new WaitUntil();
        FocusOnNewTarget();
        
    }
}
