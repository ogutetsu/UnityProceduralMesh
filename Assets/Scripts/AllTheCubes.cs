using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTheCubes : AbstractMeshGenerator
{
    [SerializeField] private Vector3[] vs = new Vector3[8];

    protected override void SetMeshNums()
    {
        numVertices = 36;
        numTriangles = 36;
    }

    protected override void SetVertices()
    {
        vertices.Add(vs[3]);
        vertices.Add(vs[1]);
        vertices.Add(vs[2]);

        vertices.Add(vs[3]);
        vertices.Add(vs[0]);
        vertices.Add(vs[1]);

        vertices.Add(vs[7]);
        vertices.Add(vs[0]);
        vertices.Add(vs[3]);

        vertices.Add(vs[7]);
        vertices.Add(vs[4]);
        vertices.Add(vs[0]);

        vertices.Add(vs[6]);
        vertices.Add(vs[3]);
        vertices.Add(vs[2]);

        vertices.Add(vs[6]);
        vertices.Add(vs[7]);
        vertices.Add(vs[3]);

        vertices.Add(vs[0]);
        vertices.Add(vs[5]);
        vertices.Add(vs[1]);

        vertices.Add(vs[0]);
        vertices.Add(vs[4]);
        vertices.Add(vs[5]);

        vertices.Add(vs[7]);
        vertices.Add(vs[5]);
        vertices.Add(vs[4]);

        vertices.Add(vs[7]);
        vertices.Add(vs[6]);
        vertices.Add(vs[5]);

        vertices.Add(vs[6]);
        vertices.Add(vs[1]);
        vertices.Add(vs[5]);

        vertices.Add(vs[6]);
        vertices.Add(vs[2]);
        vertices.Add(vs[1]);

    }

    protected override void SetTriangles()
    {
        for (int i = 0; i < numTriangles; i++)
        {
            triangles.Add(i);
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
