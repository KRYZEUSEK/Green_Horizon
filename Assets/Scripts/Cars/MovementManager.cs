using Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public GameManager gameManager;

    public List<GameObject> cars = new List<GameObject>();
    public List<GameObject> people = new List<GameObject>();

    private int lastSatisfaction = -1;

    void Update()
    {
        if (gameManager == null)
            return;

        if (gameManager.Satisfaction != lastSatisfaction)
        {
            ControllMovement();
            lastSatisfaction = gameManager.Satisfaction;
        }
    }

    public void ControllMovement()
    {
        // Najpierw wy³¹cz wszystkie obiekty
        foreach (var go in cars)
        {
            if (go != null)
                go.SetActive(false);
        }
        foreach (var go in people)
        {
            if (go != null)
                go.SetActive(false);
        }

        int s = gameManager.Satisfaction;

        if (s > 0 && s <= 25)
        {
            // Aktywuj pierwszy samochód
            if (cars.Count > 0 && cars[0] != null)
                cars[0].SetActive(true);
        }
        else if (s > 25 && s <= 50)
        {
            // Aktywuj pierwsze 3 samochody
            for (int i = 0; i < 3 && i < cars.Count; i++)
            {
                if (cars[i] != null)
                    cars[i].SetActive(true);
            }
            // Aktywuj pierwsze 2 osoby
            for (int i = 0; i < 2 && i < people.Count; i++)
            {
                if (people[i] != null)
                    people[i].SetActive(true);
            }
        }
        else if (s > 50 && s < 75)
        {
            // Aktywuj pierwsze 4 samochody
            for (int i = 0; i < 4 && i < cars.Count; i++)
            {
                if (cars[i] != null)
                    cars[i].SetActive(true);
            }
            // Aktywuj pierwsze 3 osoby
            for (int i = 0; i < 3 && i < people.Count; i++)
            {
                if (people[i] != null)
                    people[i].SetActive(true);
            }
        }
        else if (s >= 75)
        {
            // Aktywuj wszystkie samochody
            foreach (var go in cars)
            {
                if (go != null)
                    go.SetActive(true);
            }
            // Aktywuj wszystkie osoby
            foreach (var go in people)
            {
                if (go != null)
                    go.SetActive(true);
            }
        }
    }
}