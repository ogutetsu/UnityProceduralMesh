using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllThePrisms : AbstractMeshGenerator
{
    [SerializeField, Range(3, 50)]
    private int numSides = 3;

    [SerializeField] private float frontRadius;
    [SerializeField] private float backRadius;
    [SerializeField] private float length;

    [SerializeField] private Gradient gradient;

    private Vector3[] vs;


    protected override void SetMeshNums()
    {
        numVertices = 6 * numSides;
        numTriangles = 12 * (numSides - 1);
    }

    protected override void SetVertices()
    {
        vs = new Vector3[2 * numSides];
        for (int i = 0; i < numSides; i++)
        {
            float angle = 2 * (float) Mathf.PI * i / numSides;
            vs[i] = new Vector3(frontRadius * Mathf.Cos(angle), frontRadius * Mathf.Sin(angle), 0);

            vs[i + numSides] = new Vector3(backRadius * Mathf.Cos(angle), backRadius * Mathf.Sin(angle), length);
        }

        for (int i = 0; i < numSides; i++)
        {
            vertices.Add(vs[i]);
        }

        for (int i = 0; i < numSides; i++)
        {
            vertices.Add(vs[i]);
            int secondIndex = i == 0 ? 2 * numSides - 1 : numSides + i - 1;
            vertices.Add(vs[secondIndex]);
            int thirdIndex = i == 0 ? numSides - 1 : i - 1;
            vertices.Add(vs[thirdIndex]);
            vertices.Add(vs[i + numSides]);

        }

        for (int i = 0; i < numSides; i++)
        {
            vertices.Add(vs[i + numSides]);
        }
    }

    protected override void SetTriangles()
    {
        for (int i = 1; i < numSides - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(i);
        }

        for (int i = 1; i <= numSides; i++)
        {
            int val = numSides + 4 * (i - 1);

            triangles.Add(val);
            triangles.Add(val + 1);
            triangles.Add(val + 2);

            triangles.Add(val);
            triangles.Add(val + 3);
            triangles.Add(val + 1);
        }

        for (int i = 1; i < numSides - 1; i++)
        {
            triangles.Add(5 * numSides);
            triangles.Add(5 * numSides + i);
            triangles.Add(5 * numSides + i + 1);
        }
    }

    protected override void SetNormals()
    {
        Vector3 frontNormal = new Vector3(0,0,-1);
        for(int i = 0; i < numSides; i++)
        {
            normals.Add(frontNormal);
        }

        for (int i = 0; i < numSides; i++)
        {
            int index = numSides + 4 * i;
            Vector3 dirOne = vertices[index + 2] - vertices[index];
            Vector3 dirTwo = vertices[index + 3] - vertices[index];

            Vector3 normal = Vector3.Cross(dirTwo, dirOne).normalized;

            for (int n = 0; n < 4; n++)
            {
                normals.Add(normal);
            }

        }

        Vector3 backNormal = new Vector3(0, 0, -1);
        for (int i = 0; i < numSides; i++)
        {
            normals.Add(backNormal);
        }
    }

    protected override void SetTangents()
    {
        Vector4 frontTangent = new Vector4(1,0,0,-1);
        for (int i = 0; i < numSides; i++)
        {
            tangents.Add(frontTangent);
        }

        for (int i = 0; i < numSides; i++)
        {
            int index = numSides + 4 * i;
            Vector3 uDir = vertices[index] - vertices[index + 2];
            Vector4 sideTangent = uDir.normalized;
            sideTangent.w = 1;

            for (int n = 0; n < 4; n++)
            {
                tangents.Add(sideTangent);
            }

        }

        Vector4 backTangent = new Vector4(1, 0, 0, 1);
        for (int i = 0; i < numSides; i++)
        {
            tangents.Add(backTangent);
        }


    }

    protected override void SetUVs()
    {
        for (int i = 0; i < numSides; i++)
        {
            uvs.Add(vs[i]);
        }

        for (int i = 0; i < numSides; i++)
        {
            uvs.Add(new Vector2(frontRadius,0));
            uvs.Add(new Vector2(0, length));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(backRadius, length));
        }


        for (int i = 0; i < numSides; i++)
        {
            uvs.Add(vs[i + numSides]);
        }

    }

    protected override void SetVertexColors()
    {
        for (int i = 0; i < numVertices; i++)
        {
            vertexColors.Add(gradient.Evaluate((float)i/numVertices));
        }
    }

}
