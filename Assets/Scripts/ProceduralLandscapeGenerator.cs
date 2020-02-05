using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLandscapeGenerator : AbstractMeshGenerator
{
    [SerializeField] private int xResolution = 20;
    [SerializeField] private int zResolution = 20;

    [SerializeField] private float meshScale = 1;
    [SerializeField] private float yScale = 1;

    [SerializeField, Range(1, 8)] private int octaves = 1;
    [SerializeField] private float lacunarity = 2;
    [SerializeField, Range(0, 1)] private float gain = 0.5f;
    [SerializeField] private float perlinScale = 1;


    [SerializeField] private float uvScale = 1;

    [SerializeField] private Gradient gradient;

    [SerializeField] private float gradMin = -2;
    [SerializeField] private float gradMax = 5;

    [SerializeField] private FallOffType type;
    [SerializeField] private float falloffSize;
    [SerializeField] private float seaLevel;

    protected override void SetMeshNums()
    {
        numVertices = (xResolution + 1) * (zResolution + 1);
        numTriangles = 6 * xResolution * zResolution;
    }

    protected override void SetVertices()
    {
        float xx, y, zz = 0;
        Vector3[] vs = new Vector3[numVertices];


        NoiseGenerator noise = new NoiseGenerator(octaves, lacunarity, gain, perlinScale);

        for (int z = 0; z <= zResolution; z++)
        {
            for (int x = 0; x <= xResolution; x++)
            {
                xx = ((float)x / xResolution) * meshScale;
                zz = ((float)z / zResolution) * meshScale;

                y = yScale * noise.GetFractalNoise(xx, zz);
                y = FallOff((float) x, y, (float) z);
                vertices.Add(new Vector3(xx, y, zz));
            }
            
        }
    }

    private float FallOff(float x, float height, float z)
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
        int triCount = 0;
        for (int z = 0; z < zResolution; z++)
        {
            for (int x = 0; x < xResolution; x++)
            {
                triangles.Add(triCount);
                triangles.Add(triCount + xResolution + 1);
                triangles.Add(triCount + 1);

                triangles.Add(triCount + 1);
                triangles.Add(triCount + xResolution + 1);
                triangles.Add(triCount + xResolution + 2);

                triCount++;
            }

            triCount++;
        }

        
    }

    protected override void SetNormals()
    {
        SetGeneralNormals();
    }

    protected override void SetTangents()
    {
        SetGeneralTangents();
    }

    protected override void SetUVs()
    {

        for (int z = 0; z <= zResolution; z++)
        {
            for (int x = 0; x <= xResolution; x++)
            {
                uvs.Add(new Vector2(x / (uvScale * xResolution), z / (uvScale * zResolution)));
            }
        }
    }

    protected override void SetVertexColors()
    {
        
        float diff = gradMax - gradMin;
        for (int i = 0; i < numVertices; i++)
        {
            vertexColors.Add(gradient.Evaluate((vertices[i].y - gradMin) /diff));
        }

    }
}
