using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Stamp : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public bool IsDragged { private set; get; } = false;
    public bool CanBeMoved { private set; get; } = true;
    public Sprite stampedSprite;

    private Sprite defaultSprite;

    private void Start() {
        defaultSprite = GetComponent<Image>().sprite;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        IsDragged = true;
    }

    public void OnDrag(PointerEventData eventData) {
        if (CanBeMoved == false) { return; }

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        IsDragged = false;
        GameObject[] dropAreas = GameObject.FindGameObjectsWithTag("DropArea");
        
        if (dropAreas.Length == 0) { return; }

        GameObject closestDropArea = dropAreas[0];
        float closestDistance = Vector3.Distance(transform.position, closestDropArea.transform.position);

        foreach (GameObject dropArea in dropAreas) {
            float distance = Vector3.Distance(transform.position, dropArea.transform.position);

            if (distance < closestDistance) {
                closestDropArea = dropArea;
            }
        }

        bool isWithinBounds = RectTransformUtility.RectangleContainsScreenPoint(
            closestDropArea.GetComponent<RectTransform>(),
            Input.mousePosition
        );

        if (isWithinBounds == false) { return; }
        
        transform.position = closestDropArea.transform.position;

        if (closestDropArea.TryGetComponent(out DropArea da)) {
            da.onDropEvent?.Invoke();
        }

        ChangeSprite(stampedSprite);
    }

    public void ResetStamp() {
        transform.localPosition = Vector3.zero;

        IsDragged = false;
        CanBeMoved = true;

        ChangeSprite(defaultSprite);
    }

    private void ChangeSprite(Sprite sprite) {
        if (sprite != null) {
            GetComponent<Image>().sprite = sprite;
        }
    }
}