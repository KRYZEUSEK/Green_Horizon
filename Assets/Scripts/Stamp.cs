using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Stamp : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    [Tooltip("Distance to attract the stamp to the drop area")]
    [SerializeField] private float stampAttractionDistance = 200f;
    [Tooltip("Speed at which the stamp returns to its original position")]
    [SerializeField] private float returnSpeed = 10f;
    [Tooltip("Cooldown time before the stamp can be moved again")]
    [SerializeField] private float cooldownTime = 1f;
    [SerializeField] private AudioClip stampSound;

    public bool IsDragged { private set; get; } = false;
    public bool CanBeMoved { private set; get; } = true;
    public Sprite stampedSprite;

    private Sprite defaultSprite;
    private bool isMovingBack;

    private void Start() {
        defaultSprite = GetComponent<Image>().sprite;
    }

    private void Update() {
        if (isMovingBack) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, 0), Time.deltaTime * returnSpeed);

            if (Vector3.Distance(transform.localPosition, new Vector3(0, 0, 0)) < 0.1f) {
                isMovingBack = false;
                Invoke(nameof(EnableMovement), cooldownTime);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        IsDragged = true;
    }

    public void OnDrag(PointerEventData eventData) {
        if (CanBeMoved == false || isMovingBack) { return; }

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (CanBeMoved == false || isMovingBack) { return; }

        IsDragged = false;
        GameObject[] dropAreas = GameObject.FindGameObjectsWithTag("DropArea");

        if (dropAreas.Length == 0) { return; }

        GameObject closestDropArea = dropAreas[0];
        float closestDistance = Vector3.Distance(transform.position, closestDropArea.transform.position);

        foreach (GameObject dropArea in dropAreas) {
            float distance = Vector3.Distance(transform.position, dropArea.transform.position);

            if (distance < closestDistance) {
                closestDropArea = dropArea;
                closestDistance = distance;
            }
        }

        //bool isWithinBounds = RectTransformUtility.RectangleContainsScreenPoint(
        //    closestDropArea.GetComponent<RectTransform>(),
        //    Input.mousePosition
        //);

        //if (isWithinBounds == false) { return; }

        //Debug.Log($"Closest Drop Area: {closestDropArea.name} - Distance: {closestDistance}");
        if (closestDistance > stampAttractionDistance) { return; }

        transform.position = closestDropArea.transform.position;

        if (closestDropArea.TryGetComponent(out DropArea da)) {
            da.onDropEvent?.Invoke();
            MainSoundsManager.Instance.PlayClip(stampSound);
        }

        ChangeSprite(stampedSprite);
        CanBeMoved = false;
    }

    public void ResetStamp() {
        isMovingBack = true;
        CanBeMoved = false;
        IsDragged = false;

        ChangeSprite(defaultSprite);
    }

    public void EnableMovement() {
        CanBeMoved = true;
    }

    private void ChangeSprite(Sprite sprite) {
        if (sprite != null) {
            GetComponent<Image>().sprite = sprite;
        }
    }
}