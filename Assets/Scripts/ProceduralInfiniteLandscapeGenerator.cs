using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralInfiniteLandscapeGenerator : MonoBehaviour
{

    [SerializeField] private ProcGenChunk landscapePrefab;

    [SerializeField] private Material material;

    [SerializeField] private int xResolution = 20;
    [SerializeField] private int zResolution = 20;

    [SerializeField] private float meshScale = 1;
    [SerializeField] private float yScale = 1;

    [SerializeField, Range(1, 8)] private int octaves = 1;
    [SerializeField] private float lacunarity = 2;
    [SerializeField, Range(0, 1)] private float gain = 0.5f;
    [SerializeField] private float perlinScale = 1;


    void Awake()
    {
        ProcGenChunk topLeft = CreateTerrainChunk(new Vector2(0, 2 * zResolution));
        ProcGenChunk topMiddle = CreateTerrainChunk(new Vector2(xResolution, 2 * zResolution));
        ProcGenChunk topRight = CreateTerrainChunk(new Vector2(2 * xResolution, 2 * zResolution));

        ProcGenChunk midLeft = CreateTerrainChunk(new Vector2(0,  zResolution));
        ProcGenChunk middle = CreateTerrainChunk(new Vector2(xResolution,  zResolution));
        ProcGenChunk midRight = CreateTerrainChunk(new Vector2(2*xResolution, zResolution));

        ProcGenChunk bottomLeft = CreateTerrainChunk(new Vector2(0, 0));
        ProcGenChunk bottomMiddle = CreateTerrainChunk(new Vector2(xResolution, 0));
        ProcGenChunk bottomRight = CreateTerrainChunk(new Vector2(2*xResolution, 0));


    }

    private ProcGenChunk CreateTerrainChunk(Vector2 position)
    {
        ProcGenChunk chunk = Instantiate(landscapePrefab);
        chunk.InitInfiniteLandScape(material, xResolution, zResolution, meshScale, yScale, octaves, lacunarity, gain, perlinScale, position);
        chunk.transform.SetParent(transform);
        return chunk;
    }

}
