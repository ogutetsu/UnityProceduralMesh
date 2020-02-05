using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLandscapeGenerator : AbstractMeshGenerator
{
    [SerializeField] private int xResolution = 20;
    [SerializeField] private int zResolution = 20;

    [SerializeField] private float meshScale = 1;
    [SerializeField] private float yScale = 1;

    [SerializeField] private float meshHeight = 1;

    [SerializeField, Range(1, 8)] private int octaves = 1;
    [SerializeField] private float lacunarity = 2;
    [SerializeField, Range(0, 1)] private float gain = 0.5f;
    [SerializeField] private float perlinScale = 1;


    [SerializeField] private bool uvFollowSurface;
    [SerializeField] private float uvScale = 1;
    [SerializeField] private float numTexPerSquare = 1;
    private int i;

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

        //Vector2[] uvArray = new Vector2[numVertices];
        //for (int i = 0; i < resolution; i++)
        //{

        //    if (uvFollowSurface)
        //    {
        //        uvArray[i] = new Vector2(i * numTexPerSquare / uvScale, 1);
        //        uvArray[i + resolution] = new Vector2(i * numTexPerSquare / uvScale, 0);
        //    }
        //    else
        //    {
        //        uvArray[i] = new Vector2(vertices[i].x / uvScale, vertices[i].y / uvScale);
        //        uvArray[i + resolution] = new Vector2(vertices[i].x, vertices[i + resolution].y);
        //    }
        //}

        //uvs.AddRange(uvArray);
    }

    protected override void SetVertexColors()
    {
    }
}
