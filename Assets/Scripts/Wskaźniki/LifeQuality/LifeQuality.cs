using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeQuality : MonoBehaviour
{
    [SerializeField] private float startingLifeQuality;
    public float currentLifeQuality { get; private set; }

    private void Awake()
    {
        currentLifeQuality = startingLifeQuality;
    }

    private void Start()
    {
    }

    public void LifeQualityDown(float _damage)
    {
        currentLifeQuality = Mathf.Clamp(currentLifeQuality - _damage, 0, startingLifeQuality);
    }
    public void LifeQualityUp(float _value)
    {
        currentLifeQuality = Mathf.Clamp(currentLifeQuality + _value, 0, startingLifeQuality);
    }

}
