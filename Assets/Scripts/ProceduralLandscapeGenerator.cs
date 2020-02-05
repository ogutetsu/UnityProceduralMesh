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

                vertices.Add(new Vector3(xx, y, zz));
            }
            
        }
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
