using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Budget : MonoBehaviour
{
    [SerializeField] private float startingBudget;
    public float currentBudget { get; private set; }

    private void Awake()
    {
        currentBudget = startingBudget;
    }

    private void Start()
    {
    }
    
    public void BudgetDown(float _damage)
    {
        currentBudget = Mathf.Clamp(currentBudget - _damage, 0, startingBudget);
    }
    public void BudgetUp(float _value)
    {
        currentBudget = Mathf.Clamp(currentBudget + _value, 0, startingBudget);
    }

}
