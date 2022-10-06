using UnityEngine;

public class TerrainChecker
{
    public string GetLayerName(Vector3 playerPosition, Terrain terrain)
    {
        float[] cellMix = GetLayerMix(playerPosition, terrain);
        float density = 0;
        int index = 0;

        for (int i = 0; i < cellMix.Length; i++)
        {
            if (cellMix[i] > density)
            {
                index = i;
                density = cellMix[i];
            }
        }

        return terrain.terrainData.terrainLayers[index].name;
    }

    private float[] GetLayerMix(Vector3 playerPosition, Terrain terrain)
    {
        Vector3 terrainPosition = terrain.transform.position;
        TerrainData terrainData = terrain.terrainData;

        int mapX = Mathf.RoundToInt((playerPosition.x - terrainPosition.x) / terrainData.size.x * terrainData.alphamapWidth);
        int mapZ = Mathf.RoundToInt((playerPosition.z - terrainPosition.z) / terrainData.size.z * terrainData.alphamapHeight);
        float[,,] splatMapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
        float[] cellMix = new float[splatMapData.GetLength(2)];

        for (int i = 0; i < cellMix.Length; i++)
            cellMix[i] = splatMapData[0, 0, i];

        return cellMix;
    }
}
