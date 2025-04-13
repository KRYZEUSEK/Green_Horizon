using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Minigames.PapersGame {
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    internal class Paper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        [SerializeField] internal AudioClip pickClip, destroyClip;
        [SerializeField] internal PapersGameManager.TargetType targetType;

        private Vector2 initialPosition;

        public void OnBeginDrag(PointerEventData eventData) {
            GamesSoundsManager.Instance.PlayClip(pickClip);
            initialPosition = transform.localPosition;
        }

        public void OnDrag(PointerEventData eventData) {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint
            );

            transform.position = transform.parent.TransformPoint(localPoint);
        }

        public void OnEndDrag(PointerEventData eventData) {
            GamesSoundsManager.Instance.PlayClip(pickClip);

            if (RectTransformUtility.RectangleContainsScreenPoint(
                transform.parent.GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera
            ) == false) {
                transform.localPosition = initialPosition;
            }
        }

        internal void DestroyPaper() {
            GamesSoundsManager.Instance.PlayClip(destroyClip);
            Destroy(gameObject);
        }
    }
}