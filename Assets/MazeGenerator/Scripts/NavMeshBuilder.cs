using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavMeshBuilder : MonoBehaviour
{
    private NavMeshSurface surface;

    void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
        if (surface == null)
        {
            surface = gameObject.AddComponent<NavMeshSurface>();
        }
        ConfigureNavMeshSurface();
    }

    private void ConfigureNavMeshSurface()
    {
        if (surface != null)
        {
            surface.collectObjects = CollectObjects.All;
            surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            surface.defaultArea = 0; // Walkable area
            surface.layerMask = -1; // All layers
        }
    }

    public void RebuildNavMesh()
    {
        if (surface != null)
        {
            surface.BuildNavMesh();
            Debug.Log("NavMesh rebuilt successfully");
        }
        else
        {
            Debug.LogError("NavMeshSurface not found!");
        }
    }
}