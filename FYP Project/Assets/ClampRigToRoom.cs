using System.Collections.Generic;
using UnityEngine;

public class ClampRigToRoom : MonoBehaviour
{
    [Header("List of colliders that define allowed area")]
    public List<Collider> allowedColliders = new List<Collider>();

    [Tooltip("Reference to the moving rig root (usually parent of OVR camera rig).")]
    public Transform rigRoot;

    [Tooltip("Padding from the collider surface to avoid clipping walls.")]
    public float wallPadding = 0.05f;

    void Reset()
    {
        rigRoot = transform;
    }

    void LateUpdate()
    {
        if (rigRoot == null || allowedColliders.Count == 0)
            return;

        Vector3 pos = rigRoot.position;

        // Check if inside any allowed collider
        bool insideAny = false;
        foreach (var col in allowedColliders)
        {
            if (col == null) continue;

            if (PointInsideCollider(col, pos))
            {
                insideAny = true;
                break;
            }
        }

        if (!insideAny)
        {
            // Find closest collider surface and clamp
            Vector3 closest = pos;
            float minDist = float.MaxValue;

            foreach (var col in allowedColliders)
            {
                if (col == null) continue;

                Vector3 cp = col.ClosestPoint(pos);
                float d = (cp - pos).sqrMagnitude;
                if (d < minDist)
                {
                    minDist = d;
                    closest = cp;
                }
            }

            // Apply clamp (keep Y unchanged, so teleport vertical height stays stable)
            rigRoot.position = new Vector3(
                closest.x,
                pos.y,
                closest.z
            ) + (pos - closest).normalized * wallPadding;
        }
    }

    // Works for MeshColliders, BoxColliders, SphereColliders, etc.
    private bool PointInsideCollider(Collider col, Vector3 point)
    {
        // Unity doesn't expose "Contains" for colliders generally.
        // For MeshColliders marked convex and simple shapes, we can approximate:
        Vector3 closest = col.ClosestPoint(point);
        return Vector3.Distance(closest, point) < 0.001f;
    }
}
