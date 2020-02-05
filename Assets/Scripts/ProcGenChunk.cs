using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcGenChunk : AbstractIslandMeshGenerator
{

    [SerializeField] private int xStartPos;
    [SerializeField] private int zStartPos;
    [SerializeField] private int xEndPos;
    [SerializeField] private int zEndPos;


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

        for (int z = zStartPos; z <= zEndPos; z++)
        {
            for (int x = xStartPos; x <= xEndPos; x++)
            {
                xx = ((float)x / xResolution) * meshScale;
                zz = ((float)z / zResolution) * meshScale;

                y = yScale * noise.GetFractalNoise(xx, zz);
                y = FallOff((float)x, y, (float)z);
                vertices.Add(new Vector3(xx, y, zz));
            }

        }
    }



    protected override void SetTriangles()
    {
        int triCount = 0;
        for (int z = zStartPos; z < zEndPos; z++)
        {
            for (int x = xStartPos; x < xEndPos; x++)
            {
                triangles.Add(triCount);
                triangles.Add(triCount + xResolution + 1);
                triangles.Add(triCount + 1);

                triangles.Add(triCount + 1);
                triangles.Add(triCount + xResolution + 1);
                triangles.Add(triCount + xResolution + 2);

                triCount++;
            }

            triCount++;
        }


    }

    protected override void SetNormals()
    {
        SetGeneralNormals();
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
