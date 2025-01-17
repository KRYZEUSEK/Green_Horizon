using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infrastructure : MonoBehaviour
{
    [SerializeField] private float startingInfrastructure;
    public float currentInfrastructure { get; private set; }
    private void Awake()
    {
        currentInfrastructure = startingInfrastructure;
    }
    public void InfrastructureDown(float _damage)
    {
        currentInfrastructure = Mathf.Clamp(currentInfrastructure - _damage, 0, startingInfrastructure);
    }
    public void InfrastructureUp(float _value)
    {
        currentInfrastructure = Mathf.Clamp(currentInfrastructure + _value, 0, startingInfrastructure);
    }   
}

    


