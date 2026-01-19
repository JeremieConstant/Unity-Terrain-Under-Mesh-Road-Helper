using UnityEngine;

public class MeshToTerrainDeformer : MonoBehaviour
{
    [Header("Settings")]
    public GameObject roadMeshObject; // The object with the MeshCollider
    public LayerMask roadLayer;       // Set the road to a specific layer (e.g., "Ignore Raycast" or a custom one)

    [ContextMenu("Project Mesh to Terrain")]
    public void ProjectMesh()
    {
        var terrain = GetComponent<Terrain>();
        var meshCollider = roadMeshObject.GetComponent<MeshCollider>();

        if (terrain == null || meshCollider == null)
        {
            Debug.LogError("Assign the Terrain and ensure the Road Mesh has a MeshCollider!");
            return;
        }

        TerrainData terrainData = terrain.terrainData;
        int res = terrainData.heightmapResolution;
        Vector3 terrainSize = terrainData.size;
        Vector3 terrainPos = terrain.transform.position;

        float[,] heights = terrainData.GetHeights(0, 0, res, res);

        // 1. Get the bounding box of the mesh in world space
        Bounds bounds = meshCollider.bounds;

        // 2. Convert world bounds to heightmap pixel range
        int minX = Mathf.FloorToInt(((bounds.min.x - terrainPos.x) / terrainSize.x) * (res - 1));
        int maxX = Mathf.CeilToInt(((bounds.max.x - terrainPos.x) / terrainSize.x) * (res - 1));
        int minZ = Mathf.FloorToInt(((bounds.min.z - terrainPos.z) / terrainSize.z) * (res - 1));
        int maxZ = Mathf.CeilToInt(((bounds.max.z - terrainPos.z) / terrainSize.z) * (res - 1));

        // Clamp to terrain boundaries
        minX = Mathf.Clamp(minX, 0, res - 1);
        maxX = Mathf.Clamp(maxX, 0, res - 1);
        minZ = Mathf.Clamp(minZ, 0, res - 1);
        maxZ = Mathf.Clamp(maxZ, 0, res - 1);

        int modifiedCount = 0;

        // 3. Loop only through the pixels covered by the mesh bounds
        for (int z = minZ; z <= maxZ; z++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                // Calculate world position of this specific terrain pixel
                float worldX = terrainPos.x + (x / (float)(res - 1)) * terrainSize.x;
                float worldZ = terrainPos.z + (z / (float)(res - 1)) * terrainSize.z;
                
                // Raycast from high above the mesh downwards
                Vector3 rayOrigin = new Vector3(worldX, bounds.max.y + 5f, worldZ);
                Ray ray = new Ray(rayOrigin, Vector3.down);

                if (meshCollider.Raycast(ray, out RaycastHit hit, bounds.size.y + 10f))
                {
                    // Convert hit point to normalized terrain height
                    float targetHeight = (hit.point.y - terrainPos.y) / terrainSize.y;
                    heights[z, x] = Mathf.Clamp01(targetHeight);
                    modifiedCount++;
                }
            }
        }

        // 4. Apply
        terrainData.SetHeights(0, 0, heights);
        terrain.Flush();
        Debug.Log($"Projected mesh onto terrain. Modified {modifiedCount} pixels.");
    }
}