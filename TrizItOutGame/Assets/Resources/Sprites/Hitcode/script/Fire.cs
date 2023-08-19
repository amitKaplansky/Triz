using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fire : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float currentIntensity = 1.0f;
    private float[] startIntensities = new float[0];
    [SerializeField] private ParticleSystem[] fireParticalSystems = new ParticleSystem[0];
    public float CurrentIntensity
    {
        get
        {
            return currentIntensity;
        }
        set
        {
            currentIntensity = value;
        }
    }
    private void Start()
    {
        startIntensities = new float[fireParticalSystems.Length];

        for (int i = 0; i < fireParticalSystems.Length; i++)
        {

            startIntensities[i] = fireParticalSystems[i].emission.rateOverTime.constant;
        }
    }

    private void Update()
    {
        ChangeIntensity();

    }
    private void ChangeIntensity()
    {
        for (int i = 0; i < fireParticalSystems.Length; i++)
        {
            var emission = fireParticalSystems[i].emission;
            emission.rateOverTime = currentIntensity * startIntensities[i];
        }

    }

}
