using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class NoGoZonePushback : MonoBehaviour
{
    public Transform playerRoot;          // parent you move for locomotion
    public float skin = 0.05f;            // extra push outside the face
    public bool lockY = true;             // keep player height unchanged

    private BoxCollider box;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
        box.isTrigger = true;

        if (playerRoot == null)
        {
            // Try to auto-find OVRCameraRig and use its parent if available
            var rig = FindObjectOfType<OVRCameraRig>();
            if (rig != null && rig.transform.parent != null)
                playerRoot = rig.transform.parent;
            else if (rig != null)
                playerRoot = rig.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerRoot == null) return;

        // Only react when the player's hierarchy is inside
        if (other.transform.root != playerRoot.root) return;

        Vector3 pos = playerRoot.position;

        // Convert to the box's local space (account for box center)
        Vector3 local = transform.InverseTransformPoint(pos) - box.center;
        Vector3 half = box.size * 0.5f;

        // Are we inside the box volume?
        if (Mathf.Abs(local.x) < half.x && Mathf.Abs(local.y) < half.y && Mathf.Abs(local.z) < half.z)
        {
            // Distances to the 6 faces
            float dx = half.x - Mathf.Abs(local.x);
            float dy = half.y - Mathf.Abs(local.y);
            float dz = half.z - Mathf.Abs(local.z);

            // Push out along the nearest face
            if (dx <= dy && dx <= dz)
                local.x = Mathf.Sign(local.x) * (half.x + skin);
            else if (dy <= dx && dy <= dz)
                local.y = Mathf.Sign(local.y) * (half.y + skin);
            else
                local.z = Mathf.Sign(local.z) * (half.z + skin);

            Vector3 world = transform.TransformPoint(local + box.center);
            if (lockY) world.y = pos.y;

            playerRoot.position = world;
        }
    }
}
