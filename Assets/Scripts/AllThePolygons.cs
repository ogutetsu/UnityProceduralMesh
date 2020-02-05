using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllThePolygons : AllTheUniqueVertQuads
{
    [SerializeField, Range(3,50)]
    private int numSides = 3;

    [SerializeField] private float radius;

    [SerializeField] private float xTiling = 1;
    [SerializeField] private float yTiling = 1;

    [SerializeField] private float xScroll = 1;
    [SerializeField] private float yScroll = 1;

    [SerializeField] private float angle = 0;


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
        Vector3 normal = new Vector3(0, 0, -1);
        for (int i = 0; i < numVertices; i++)
        {
            normals.Add(normal);
        }
    }

    protected override void SetTangents()
    {
    }

    protected override void SetUVs()
    {
        for (int i = 0; i < numVertices; i++)
        {
            float uvX = xTiling * vertices[i].x + xScroll;
            float uvY = yTiling * vertices[i].y + yScroll;
            Vector2 uv = new Vector2(uvX, uvY);
            uv = Quaternion.AngleAxis(angle, Vector3.forward) * uv;
            uvs.Add(uv);
        }
    }

    protected override void SetVertexColors()
    {
    }
}
