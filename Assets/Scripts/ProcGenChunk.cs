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

    private float uvScale;


    private List<Vector3> outerVertices = new List<Vector3>();
    private List<int> outerTriangles = new List<int>();
    private List<Vector3> outerUVs = new List<Vector3>();

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

        this.uvScale = uvScale;

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

            normals.Add(norms[i].normalized);
        }



    }

    protected override void SetTangents()
    {
        if (uvs.Count == 0 || normals.Count == 0)
        {
            return;
        }
        int numGeometricTriangles = outerTriangles.Count / 3;
        Vector3[] tans = new Vector3[outerVertices.Count];
        Vector3[] bitans = new Vector3[outerVertices.Count];

        int index = 0;
        for (int i = 0; i < numGeometricTriangles; i++)
        {
            int triA = outerTriangles[index];
            int triB = outerTriangles[index+1];
            int triC = outerTriangles[index+2];

            Vector2 uvA = outerUVs[triA];
            Vector2 uvB = outerUVs[triB];
            Vector2 uvC = outerUVs[triC];

            Vector3 dirA = outerVertices[triB] - outerVertices[triA];
            Vector3 dirB = outerVertices[triC] - outerVertices[triA];

            Vector2 uvDiffA = new Vector2(uvB.x - uvA.x, uvC.x - uvA.x);
            Vector2 uvDiffB = new Vector2(uvB.y - uvA.y, uvC.y - uvA.y);

            float invDet = uvDiffA.x * uvDiffB.y - uvDiffA.y * uvDiffB.x;
            if (invDet == 0)
            {
                return;
            }

            float determinant = 1f / invDet;
            Vector3 sDir = determinant * (new Vector3(uvDiffB.y * dirA.x - uvDiffB.x * dirB.x,
                               uvDiffB.y * dirA.y - uvDiffB.x * dirB.y, uvDiffB.y * dirA.z - uvDiffB.x * dirB.z));
            Vector3 tDir = determinant * (new Vector3(uvDiffA.x * dirB.x - uvDiffA.y * dirA.x,
                               uvDiffA.x * dirB.y - uvDiffA.y * dirA.y, uvDiffA.x * dirB.z - uvDiffA.y * dirA.z));

            tans[triA] += sDir;
            tans[triB] += sDir;
            tans[triC] += sDir;

            bitans[triA] += tDir;
            bitans[triB] += tDir;
            bitans[triC] += tDir;

            index += 3;
        }

        int outerWidth = xResolution + 2;
        int normalsIndex = 0;
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

            Vector3 normal = normals[normalsIndex];
            normalsIndex++;

            Vector3 tan = tans[i];

            Vector3 tangent3 = (tan - Vector3.Dot(normal, tan) * normal).normalized;
            Vector4 tangent = tangent3;

            tangent.w = Vector3.Dot(Vector3.Cross(normal, tan), bitans[i]) < 0f ? -1f : 1f;
            tangents.Add(tangent);

        }

    }

    protected override void SetUVs()
    {

        for (int z = zOuterStartPos; z <= zOuterEndPos; z++)
        {
            for (int x = xOuterStartPos; x <= xOuterEndPos; x++)
            {
                outerUVs.Add(new Vector2(x / (uvScale * xResolution), z / (uvScale * zResolution)));
                if (z >= zStartPos && z <= zEndPos && x >= xStartPos && x <= xEndPos)
                {
                    uvs.Add(new Vector2(x / (uvScale * xResolution), z / (uvScale * zResolution)));
                }

                
            }
        }
    }

    protected override void SetVertexColors()
    {


    }
}
