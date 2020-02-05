using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPolyLandscapeGenerator : AbstractIslandMeshGenerator
{
    protected override void SetMeshNums()
    {
        numTriangles = 6 * xResolution * zResolution;
        numVertices = numTriangles;
    }

    protected override void SetVertices()
    {
        NoiseGenerator noise = new NoiseGenerator(octaves, lacunarity, gain, perlinScale);

        int xx = 0;
        int zz = 0;
        bool isBottomTriangle = false;

        for (int vertexIndex = 0; vertexIndex < numVertices; vertexIndex++)
        {

            if (IsNewRow(vertexIndex))
            {
                isBottomTriangle = !isBottomTriangle;
            }

            if (!IsNewRow(vertexIndex))
            {
                if (isBottomTriangle)
                {
                    if (vertexIndex % 3 == 1)
                    {
                        xx++;
                    }
                }
                else
                {
                    if (vertexIndex % 3 == 2)
                    {
                        xx++;
                    }
                }
            }

            if (IsNewRow(vertexIndex))
            {
                xx = 0;
                if (!isBottomTriangle)
                {
                    zz++;
                }
            }

            float xVal = ((float)xx / xResolution) * meshScale;
            float zVal = ((float)zz / zResolution) * meshScale;

            float y = yScale * noise.GetFractalNoise(xVal, zVal);
            y = FallOff((float)xx, y, (float)zz);
            vertices.Add(new Vector3(xVal, y, zVal));
        }

    }

    private bool IsNewRow(int vertexIndex)
    {
        return vertexIndex % (3 * xResolution) == 0;
    }

    protected override void SetTriangles()
    {
        int triCount = 0;
        for (int z = 0; z < zResolution; z++)
        {
            for (int x = 0; x < xResolution; x++)
            {
                triangles.Add(triCount);
                triangles.Add(triCount +3 * xResolution);
                triangles.Add(triCount + 1);

                triangles.Add(triCount + 2);
                triangles.Add(triCount + 3 * xResolution + 1);
                triangles.Add(triCount + 3 * xResolution + 2);

                triCount+= 3;
            }

            triCount+= 3 * xResolution;
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
