using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator 
{

    public float GetValueNoise()
    {
        return Random.value;
    }

    public float GetPerlinNoise(float x, float z)
    {
        return Mathf.PerlinNoise(x, z);
    }

}
