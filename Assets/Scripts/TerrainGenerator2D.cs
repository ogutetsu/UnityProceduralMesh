using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator2D : AbstractMeshGenerator
{

    [SerializeField] private int resolution = 20;

    [SerializeField] private float xScale = 1;
    [SerializeField] private float yScale = 1;

    [SerializeField] private float meshHeight = 1;


    protected override void SetMeshNums()
    {
        numVertices = 2 * resolution;
        numTriangles = 6 * (resolution - 1);
    }

    protected override void SetVertices()
    {
        float x, y = 0;
        Vector3[] vs = new Vector3[numVertices];

        for (int i = 0; i < resolution; i++)
        {
            x = ((float) i / resolution) * xScale;
            y = yScale;

            vs[i] = new Vector3(x,y,0);
            vs[i+resolution] = new Vector3(x, y - meshHeight, 0);
        }
        vertices.AddRange(vs);
    }

    protected override void SetTriangles()
    {
        for (int i = 0; i < resolution - 1; i++)
        {
            triangles.Add(i);
            triangles.Add(i + resolution + 1);
            triangles.Add(i + resolution);

            triangles.Add(i);
            triangles.Add(i +   1);
            triangles.Add(i + resolution + 1);

        }
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
