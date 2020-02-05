using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcGenChunk : AbstractIslandMeshGenerator
{

    [SerializeField] private int xStartPos;
    [SerializeField] private int zStartPos;
    [SerializeField] private int xEndPos;
    [SerializeField] private int zEndPos;


    private int zOuterStartPos;
    private int zOuterEndPos;
    private int xOuterStartPos;
    private int xOuterEndPos;




    private List<Vector3> outerVertices = new List<Vector3>();
    private List<int> outerTriangles = new List<int>();


    public void InitInfiniteLandScape(Material mat, int xRes, int zRes, float meshScale, float yScale, int octaves,
        float lacunarity, float gain, float perlinScale, Vector2 startPosition)
    {
        this.material = mat;
        this.xResolution = xRes;
        this.zResolution = zRes;
        this.meshScale = meshScale;
        this.yScale = yScale;

        this.octaves = octaves;
        this.lacunarity = lacunarity;
        this.gain = gain;
        this.perlinScale = perlinScale;

        xStartPos = (int) startPosition.x;
        zStartPos = (int) startPosition.y;
        xEndPos = xStartPos + xRes;
        zEndPos = zStartPos + zRes;


        zOuterStartPos = zStartPos - 1;
        zOuterEndPos = zEndPos + 1;
        xOuterStartPos = xStartPos - 1;
        xOuterEndPos = xEndPos + 1;


        type = FallOffType.None;
    }

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

        for (int z = zOuterStartPos; z <= zOuterEndPos; z++)
        {
            for (int x = xOuterStartPos; x <= xOuterEndPos; x++)
            {
                xx = ((float)x / xResolution) * meshScale;
                zz = ((float)z / zResolution) * meshScale;

                y = yScale * noise.GetFractalNoise(xx, zz);
                y = FallOff((float)x, y, (float)z);
                Vector3 vertex = new Vector3(xx, y, zz);
                outerVertices.Add(vertex);

                if (z >= zStartPos && z <= zEndPos && x >= xStartPos && x <= xEndPos)
                {
                    vertices.Add(vertex);
                }
            }

        }
    }



    protected override void SetTriangles()
    {
        int outerTriCount = 0;
        int triCount = 0;
        for (int z = zOuterStartPos; z < zOuterEndPos; z++)
        {
            for (int x = xOuterStartPos; x < xOuterEndPos; x++)
            {
                outerTriangles.Add(outerTriCount);
                outerTriangles.Add(outerTriCount + xResolution + 3);
                outerTriangles.Add(outerTriCount + 1);

                outerTriangles.Add(outerTriCount + 1);
                outerTriangles.Add(outerTriCount + xResolution + 3);
                outerTriangles.Add(outerTriCount + xResolution + 4);

                outerTriCount++;


                if (z >= zStartPos && z < zEndPos && x >= xStartPos && x < xEndPos)
                {
                    triangles.Add(triCount);
                    triangles.Add(triCount + xResolution + 1);
                    triangles.Add(triCount + 1);

                    triangles.Add(triCount + 1);
                    triangles.Add(triCount + xResolution + 1);
                    triangles.Add(triCount + xResolution + 2);

                    triCount++;
                }
            }

            if (z >= zStartPos && z < zEndPos)
            {
                triCount++;
            }
        }


    }

    protected override void SetNormals()
    {
        int numGeometricTriangles = outerTriangles.Count / 3;
        Vector3[] norms = new Vector3[outerVertices.Count];
        int index = 0;
        for (int i = 0; i < numGeometricTriangles; i++)
        {
            int triA = outerTriangles[index];
            int triB = outerTriangles[index+1];
            int triC = outerTriangles[index+2];

            Vector3 dirA = outerVertices[triB] - outerVertices[triA];
            Vector3 dirB = outerVertices[triC] - outerVertices[triA];

            Vector3 normal = Vector3.Cross(dirA, dirB);

            norms[triA] += normal;
            norms[triB] += normal;
            norms[triC] += normal;

            index += 3;

        }

        int outerWidth = xResolution + 2;
        for (int i = 0; i < outerVertices.Count; i++)
        {
            if (i % (outerWidth + 1) == 0 || i % (outerWidth + 1) == outerWidth)
            {
                continue;
            }

            if (i <= outerWidth || i >= outerVertices.Count - outerWidth)
            {
                continue;
            }
        }



    }

    protected override void SetTangents()
    {
        SetGeneralTangents();
    }

    protected override void SetUVs()
    {

        //for (int z = 0; z <= zResolution; z++)
        //{
        //    for (int x = 0; x <= xResolution; x++)
        //    {
        //        uvs.Add(new Vector2(x / (uvScale * xResolution), z / (uvScale * zResolution)));
        //    }
        //}
    }

    protected override void SetVertexColors()
    {


    }
}
