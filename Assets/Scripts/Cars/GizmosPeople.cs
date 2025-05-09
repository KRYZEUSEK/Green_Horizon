using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosPeople : MonoBehaviour
{
    private float waypointRadius = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextIndex(i); // Pobierz indeks nastêpnego waypointu
            Gizmos.color = Color.blue;

            // Rysuj sferê na pozycji waypointu
            Gizmos.DrawSphere(GetWaypoint(i), waypointRadius);

            // Rysuj liniê miêdzy bie¿¹cym waypointem a nastêpnym
            Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
        }
    }

    private Vector3 GetWaypoint(int i)
    {
        return transform.GetChild(i).position;
    }

    private int GetNextIndex(int i)
    {

        if (i >= transform.childCount - 1)
            return i;

        return i + 1;
    }
}
