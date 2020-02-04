using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllThePolygons : AllTheUniqueVertQuads
{
    [SerializeField, Range(3,50)]
    private int numSides = 3;

    [SerializeField] private float radius;


    protected override void SetMeshNums()
    {
        numVertices = numSides;
        numTriangles = 3 * (numSides - 2);
    }

    protected override void SetVertices()
    {
        for (int i = 0; i < numSides; i++)
        {
            float angle = 2 * (float)Math.PI * i / numSides;
            vertices.Add(new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0));
        }
    }

    protected override void SetTriangles()
    {
        for (int i = 1; i < numSides - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i+1);
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
