using TMPro;
using UnityEngine;

public class ProximityLogger : MonoBehaviour
{
    [Header("Who is the player? (HMD or rig root)")]
    public Transform player;
    public GameObject RootPlayer; // If null, tries Camera.main, then "CenterEyeAnchor"

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

    public bool _isNear = false;
    private Renderer _renderer;

    [Header("Zone Actions / UI")]
    public Transform SpawnPoint;
    public GameObject RestrictedZoneCanvas;
    public GameObject Stool;
    public TextMeshProUGUI RestrictedZoneInstructions;

    [Header("Stool Follow Options")]
    [Tooltip("If true, once the stool is placed it follows the player on XZ only while the player is inside the zone.")]
    public bool followStoolInZone = true;

    // Internals
    public bool stoolPlaced;
    public bool CollectFirstEvidenceSoundPlayed = false;
    private float _stoolFixedY; // keep Y locked while following

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
                var centerEye = GameObject.Find("CenterEyeAnchor");
                if (centerEye != null) player = centerEye.transform;
            }
            if (player == null)
                Debug.LogWarning("[ProximityLogger] No player assigned and none auto-found. Please assign the HMD or rig root.");
        }

        // Cache Y for stool if assigned
        if (Stool != null)
            _stoolFixedY = Stool.transform.position.y;
    }
    public float dist;

    private void Update()
    {
        if (player == null) return;

         dist = GetDistance(player.position);

        // --- Zone enter/exit detection ---
        if (!_isNear && dist <= enterRadius )
        {
            _isNear = true;
            if(!stoolPlaced)
            {
                Debug.Log($"[Proximity] ENTER near '{name}'. Dist = {dist:F2} m");
                if (RootPlayer != null && SpawnPoint != null)
                {
                    RootPlayer.transform.position = SpawnPoint.position;
                    RootPlayer.transform.rotation = SpawnPoint.rotation;
                }
                if (RestrictedZoneCanvas != null) RestrictedZoneCanvas.SetActive(true);
                if (SoundsManager.Instance != null)
                {
                    SoundsManager.Instance.PlayRoomEnttryDeniedSound();
                    SoundsManager.Instance.PlayPressXtoMovein();
                }
            }
          
        }
        else if (_isNear && dist >= exitRadius)
        {
            _isNear = false;
            Debug.Log($"[Proximity] EXIT near '{name}'. Dist = {dist:F2} m");
        }
        else if (_isNear && dist <= enterRadius && !CollectFirstEvidenceSoundPlayed)
        {
            if (SoundsManager.Instance != null)
                SoundsManager.Instance.PlayCollectFirstEvidence();
            CollectFirstEvidenceSoundPlayed = true;
            _isNear = true;
            
        }

        // --- Place stool with X button ---
        if (OculusInput.GetButtonDownX() && !stoolPlaced)
        {
            if (Stool != null) Stool.SetActive(true);
            if (RestrictedZoneInstructions != null)
                RestrictedZoneInstructions.text = "Great!, now you can visit this area";

            stoolPlaced = true;
            if (RestrictedZoneCanvas != null) RestrictedZoneCanvas.SetActive(false);
            if (SoundsManager.Instance != null)
                SoundsManager.Instance.PlayMoveIntoRoom();

            // Lock the stool's Y at the moment it's placed
            if (Stool != null)
                _stoolFixedY = Stool.transform.position.y;
        }

        // --- Follow behavior: only when inside the zone and stool has been placed ---
        if (followStoolInZone && stoolPlaced && _isNear && Stool != null)
        {
            Vector3 p = player.position;
            float y = _stoolFixedY; // keep Y fixed
            Stool.transform.position = new Vector3(p.x, y, p.z);
        }
        // When _isNear becomes false (exit), stool stops updating and stays where it is.
    }

    private float GetDistance(Vector3 playerPos)
    {
        if (ignoreY) playerPos.y = 0f;

        Vector3 targetPoint;

        if (useRendererBounds && _renderer != null)
        {
            var b = _renderer.bounds;
            if (ignoreY)
            {
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

        Vector3 center = transform.position;
        if (ignoreY) center.y = 0f;

        Gizmos.color = new Color(0f, 1f, 0f, 0.35f);
        Gizmos.DrawWireSphere(center, enterRadius);

        Gizmos.color = new Color(1f, 0f, 0f, 0.35f);
        Gizmos.DrawWireSphere(center, exitRadius);

        var r = GetComponent<Renderer>();
        if (r != null)
        {
            Gizmos.color = new Color(0f, 0.6f, 1f, 0.25f);
            var b = r.bounds;
            Gizmos.DrawWireCube(b.center, b.size);
        }
    }
}
