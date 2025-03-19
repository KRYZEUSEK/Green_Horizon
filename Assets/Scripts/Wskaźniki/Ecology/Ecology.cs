using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ecology : MonoBehaviour
{
    [SerializeField] private float startingEcology;
    public float currentEcology { get; private set; }

    private void Awake()
    {
        currentEcology = startingEcology;
    }

    private void Start()
    {
    }

    public void EcologyDown(float _damage)
    {
        currentEcology = Mathf.Clamp(currentEcology - _damage, 0, startingEcology);
    }
    public void EcologyUp(float _value)
    {
        currentEcology = Mathf.Clamp(currentEcology + _value, 0, startingEcology);
    }

}

