using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    [SerializeField] private float startingOrder;
    public float currentOrder { get; private set; }

    private void Awake()
    {
        currentOrder = startingOrder;
    }

    private void Start()
    {
    }

    public void OrderDown(float _damage)
    {
        currentOrder = Mathf.Clamp(currentOrder - _damage, 0, startingOrder);
    }
    public void OrderUp(float _value)
    {
        currentOrder = Mathf.Clamp(currentOrder + _value, 0, startingOrder);
    }

}