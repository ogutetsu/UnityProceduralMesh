using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractIslandMeshGenerator : AbstractMeshGenerator
{
    [SerializeField] protected int xResolution = 20;
    [SerializeField] protected int zResolution = 20;

    [SerializeField] protected float meshScale = 1;
    [SerializeField] protected float yScale = 1;

    [SerializeField, Range(1, 8)] protected int octaves = 1;
    [SerializeField] protected float lacunarity = 2;
    [SerializeField, Range(0, 1)] protected float gain = 0.5f;
    [SerializeField] protected float perlinScale = 1;
    [SerializeField] protected FallOffType type;
    [SerializeField] protected float falloffSize;
    [SerializeField] protected float seaLevel;



    protected float FallOff(float x, float height, float z)
    {
        x = x - xResolution / 2f;
        z = z - zResolution / 2f;

        float falloff = 0;
        switch (type)
        {
            case FallOffType.None:
                return height;
            case FallOffType.Circular:
                falloff = Mathf.Sqrt(x * x + z * z) / falloffSize;
                return GetHeight(falloff, height);
            case FallOffType.RoundedSquare:
                falloff = Mathf.Sqrt(x * x * x * x * z * z * z * z) / falloffSize;
                return GetHeight(falloff, height);
            default:
                return height;

        }

    }

    private float GetHeight(float falloff, float height)
    {
        if (falloff < 1)
        {
            falloff = falloff * falloff * (3 - 2 * falloff);
            height = height - falloff * (height - seaLevel);
        }
        else
        {
            height = seaLevel;
        }

        return height;
    }


    protected override void SetTriangles()
    {
    }

    protected override void SetNormals()
    {
    }

    protected override void SetTangents()
    {
    }

    protected override void SetUVs()
    {
    }

    protected override void SetVertexColors()
    {
    }
}
