using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour {
    private float moveDuration = 1f;
    private float moveElapsed = 0f;

    private Transform target;
    private Vector3 moveStart;
    private Vector3 moveEnd;

    private bool isMoving;

    void Update() {
        if (isMoving) {
            moveElapsed += Time.deltaTime;

            float distancePercentage = Mathf.Clamp01(moveElapsed / moveDuration);

            transform.position = Vector3.Lerp(moveStart, moveEnd, distancePercentage);
            
            if (distancePercentage >= 1f) {
                isMoving = false;
            }
        }
    }

    public void SetDuration(float duration) {
        moveDuration = Mathf.Clamp(duration, .0001f, float.MaxValue);
    }

    public void MoveTo(Transform newTarget) {
        if (isMoving) { return; }

        target = newTarget;
        moveElapsed = 0f;
        moveStart = transform.position;
        moveEnd = target.position;
        isMoving = true;
    }
}