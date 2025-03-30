using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DragDrop : MonoBehaviour
{
    Vector3 offset;
    public string destinationTag = "DropArea"; // tag dodany w pola naszych decyzji, tam chcemy podmieniæ sprite po upuszczeniu stamp
    private Vector3 originalScale; // Pocz¹tkowa skala stampu, zmniejsza siê im bli¿ej "DropArea", ¿eby dawaæ wra¿enie przybijania stampu 
    public float minScale = 0.8f; // minimalna skala do jakiej stamp, siê zmniejszy przy zbli¿eniu 
    public float maxDistance = 2f;

    private PolygonCollider2D polygonCollider; // collider stampa 
    private SpriteRenderer spriteRenderer; //Renderer (miejsca gdzie upuszczamy) 
    public Sprite Stamped; // to co bêdzie po podmianie(upuszczeniu)  

    public UnityEvent DecisionEvent = new UnityEvent();
    private bool isDropped = false; // Flaga œledz¹ca, czy stamp zosta³ upuszczony na DropArea

    void Start()
    {
        if (DecisionEvent == null)
            DecisionEvent = new UnityEvent();

        DecisionEvent.AddListener(DoSth);

        originalScale = transform.localScale;
        polygonCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        if (isDropped)
        {
            Debug.Log("Stamp zosta³ ju¿ upuszczony, nie mo¿na go ruszaæ.");
            return; // Jeœli stamp zosta³ upuszczony, nie pozwól na dalsze przeci¹ganie
        }

        offset = transform.position - MouseWorldPosition();
        if (polygonCollider != null)
        {
            polygonCollider.enabled = false;
        }
    }

    void OnMouseDrag()
    {
        if (isDropped)
        {
            Debug.Log("Stamp zosta³ ju¿ upuszczony, nie mo¿na go ruszaæ.");
            return; // Jeœli stamp zosta³ upuszczony, nie pozwól na dalsze przeci¹ganie
        }

        transform.position = MouseWorldPosition() + offset; // przesuwanie myszk¹
        AdjustScaleBasedOnProximity(); // wywo³anie, metody zmniejszaj¹cej skalê
    }

    void OnMouseUp()
    {
        if (isDropped)
        {
            Debug.Log("Stamp zosta³ ju¿ upuszczony, nie mo¿na go ruszaæ.");
            return; // Jeœli stamp zosta³ upuszczony, nie pozwól na dalsze przeci¹ganie
        }

        GameObject closestDropArea = FindClosestDropArea(); // szukanie nabli¿szego punktu decyzji 

        if (closestDropArea != null)
        {
            transform.position = closestDropArea.transform.position;

            SpriteRenderer hitRenderer = closestDropArea.GetComponent<SpriteRenderer>(); // pobranie renderera z miejsca upuszczenia piecz¹tki 
            
            if (hitRenderer != null)
            {
                hitRenderer.sprite = Stamped; // podmiana na sprite podbitego 
                
                isDropped = true; // Ustaw flagê, aby zablokowaæ dalsze przeci¹ganie
                Debug.Log("Stamp zosta³ upuszczony na DropArea.");
            }

            if (closestDropArea.TryGetComponent(out DropArea dropArea)) 
            {
                dropArea.onDropEvent?.Invoke();
            }
        }

        transform.localScale = originalScale;
        if (polygonCollider != null)
        {
            StartCoroutine(EnableColliderWithDelay()); //tutaj,¿eby nie siê nie psu³o (nie wraca³o do poprzedniego miejsca upuszczenia) 
        }
    }

    IEnumerator EnableColliderWithDelay()
    {
        yield return new WaitForEndOfFrame();
        polygonCollider.enabled = true;
    }

    void AdjustScaleBasedOnProximity() //metoda, która zmniejsza skalê, ¿eby siê wydawa³o, ¿e podbijamy. Im bli¿ej, tagu DropArea tym mniejsza skala 
    {
        GameObject closestDropArea = FindClosestDropArea();
        if (closestDropArea == null) return;

        float distance = Vector3.Distance(transform.position, closestDropArea.transform.position);
        float scaleFactor = Mathf.Lerp(1, minScale, Mathf.Clamp01(1 - (distance / maxDistance)));
        transform.localScale = originalScale * scaleFactor;
    }

    GameObject FindClosestDropArea()
    {
        GameObject[] dropAreas = GameObject.FindGameObjectsWithTag(destinationTag);
        if (dropAreas.Length == 0)
            return null;

        float closestDistance = maxDistance;
        GameObject closestDropArea = null;

        foreach (var dropArea in dropAreas)
        {
            float distance = Vector3.Distance(transform.position, dropArea.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestDropArea = dropArea;
            }
        }

        return closestDropArea; // Jeœli ¿adna `DropArea` nie spe³nia warunku, zwróci `null`
    }

    Vector3 MouseWorldPosition() //vector 3 znalezienie pozycji myszki 
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    private void DoSth()
    {
        Debug.Log("Decyzja Podjêta");
    }
}
