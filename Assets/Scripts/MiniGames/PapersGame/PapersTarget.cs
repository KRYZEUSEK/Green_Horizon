using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Minigames.PapersGame {
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    internal class PapersTarget : MonoBehaviour {
        [SerializeField] internal PapersGameManager.TargetType targetType;
        [SerializeField] internal UnityEvent onDragOverEvent = new UnityEvent();

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.TryGetComponent(out Paper paper)) {
                if (paper.targetType != targetType) { return; }

                onDragOverEvent?.Invoke();
                paper.DestroyPaper();
                PapersGameManager.Instance.ScorePoint();
            }
        }
    }
}