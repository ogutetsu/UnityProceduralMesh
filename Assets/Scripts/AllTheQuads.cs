﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTheQuads : AbstractMeshGenerator
{

    [SerializeField] private Vector3[] vs = new Vector3[4];
    [SerializeField] private Vector2[] flexibleUVs = new Vector2[4];

    protected override void SetMeshNums()
    {
        numVertices = 4;
        numTriangles = 6;
    }


    protected override void SetVertices()
    {
        vertices.AddRange(vs);
    }

    protected override void SetTriangles()
    {
        triangles.Add(0);
        triangles.Add(3);
        triangles.Add(2);

        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(3);
    }

    protected override void SetNormals()
    {

    }

    protected override void SetTangents()
    {

    }

    protected override void SetUVs()
    {
        uvs.AddRange(flexibleUVs);
    }

    protected override void SetVertexColors()
    {

    }


}
