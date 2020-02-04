using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTheSharedVert : AbstractMeshGenerator
{
    [SerializeField] private Vector3[] vs = new Vector3[4];

    protected override void SetMeshNums()
    {
        numVertices = 4;
        numTriangles = 12;
    }

    protected override void SetVertices()
    {
        vertices.AddRange(vs);
    }

    protected override void SetTangents()
    {
        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(1);

        triangles.Add(0);
        triangles.Add(3);
        triangles.Add(2);

        triangles.Add(2);
        triangles.Add(3);
        triangles.Add(1);

        triangles.Add(1);
        triangles.Add(3);
        triangles.Add(0);

    }

    protected override void SetNormals()
    {
    }

    

    protected override void SetTriangles()
    {
    }

    protected override void SetUVs()
    {
    }

    protected override void SetVertexColors()
    {
    }

    
}
