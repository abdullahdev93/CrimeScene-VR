using TMPro;
using UnityEngine;

public class ProximityLogger : MonoBehaviour
{
    [Header("Who is the player? (HMD or rig root)")]
    public Transform player;
    public GameObject RootPlayer;// If null, tries Camera.main, then "CenterEyeAnchor"

    [Header("Proximity Settings")]
    [Tooltip("Enter when distance <= this value")]
    public float enterRadius = 1.5f;
    [Tooltip("Exit when distance >= this value (should be >= enterRadius)")]
    public float exitRadius = 2.0f;
    [Tooltip("Ignore vertical difference (useful for stairs/ramps).")]
    public bool ignoreY = true;

    [Header("Measuring Target")]
    [Tooltip("If true, measure to the cube's renderer bounds (closest point). If false, measure to transform.position.")]
    public bool useRendererBounds = true;

    [Header("Debug")]
    public bool drawGizmos = true;

    private bool _isNear = false;
    private Renderer _renderer;
    public Transform SpawnPoint;
    public GameObject RestrictedZoneCanvas;
    public GameObject Stool;
    public TextMeshProUGUI RestrictedZoneInstructions;

    private void Awake()
    {
        if (exitRadius < enterRadius)
        {
            Debug.LogWarning("[ProximityLogger] exitRadius < enterRadius. Adjusting exitRadius to match.");
            exitRadius = enterRadius;
        }

        _renderer = GetComponent<Renderer>();

        if (player == null)
        {
            if (Camera.main != null) player = Camera.main.transform;
            if (player == null)
            {
                // Fallback for OVRCameraRig hierarchy if MainCamera tag isn't set.
                var centerEye = GameObject.Find("CenterEyeAnchor");
                if (centerEye != null) player = centerEye.transform;
            }

            if (player == null)
                Debug.LogWarning("[ProximityLogger] No player assigned and none auto-found. Please assign the HMD or rig root.");
        }
    }
    public bool stoolPlaced;
    private void Update()
    {
        //RootPlayer.transform.localPosition = new Vector3(-1f, 0.374f,  5.326f);
        if (player == null) return;

        float dist = GetDistance(player.position);

        if (!_isNear && dist <= enterRadius && !stoolPlaced)
        {
            _isNear = true;
            Debug.Log($"[Proximity] ENTER near '{name}'. Dist = {dist:F2} m");
            RootPlayer.transform.position = SpawnPoint.position;
            RootPlayer.transform.rotation = SpawnPoint.rotation;
            RestrictedZoneCanvas.SetActive(true);
        }
        else if (_isNear && dist >= exitRadius)
        {
            _isNear = false;
            Debug.Log($"[Proximity] EXIT near '{name}'. Dist = {dist:F2} m");
        }
        if(OculusInput.GetButtonDownX() && !stoolPlaced)
        {
            Stool.SetActive(true);
            RestrictedZoneInstructions.text = "Great!, now you can visit this area";
            stoolPlaced = true;
            RestrictedZoneCanvas.SetActive(false);
        }
    }

    private float GetDistance(Vector3 playerPos)
    {
        if (ignoreY) playerPos.y = 0f;

        Vector3 targetPoint;

        if (useRendererBounds && _renderer != null)
        {
            // Closest point on the cube's bounds
            var b = _renderer.bounds;
            if (ignoreY)
            {
                // Project bounds center to ground for more stable Y-agnostic distance
                var c = b.center; c.y = 0f;
                // Approximate by clamping XZ only
                var p = playerPos;
                var min = b.min; var max = b.max;
                min.y = 0f; max.y = 0f;
                float cx = Mathf.Clamp(p.x, min.x, max.x);
                float cz = Mathf.Clamp(p.z, min.z, max.z);
                targetPoint = new Vector3(cx, 0f, cz);
            }
            else
            {
                targetPoint = b.ClosestPoint(playerPos);
            }
        }
        else
        {
            targetPoint = transform.position;
            if (ignoreY) targetPoint.y = 0f;
        }

        return Vector3.Distance(playerPos, targetPoint);
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        // Where we draw from (center)
        Vector3 center = transform.position;
        if (ignoreY) center.y = 0f;

        // Enter radius
        Gizmos.color = new Color(0f, 1f, 0f, 0.35f);
        Gizmos.DrawWireSphere(center, enterRadius);

        // Exit radius
        Gizmos.color = new Color(1f, 0f, 0f, 0.35f);
        Gizmos.DrawWireSphere(center, exitRadius);

        // Approximate bounds (for visualization only)
        var r = GetComponent<Renderer>();
        if (r != null)
        {
            Gizmos.color = new Color(0f, 0.6f, 1f, 0.25f);
            var b = r.bounds;
            Gizmos.DrawWireCube(b.center, b.size);
        }
    }
}
