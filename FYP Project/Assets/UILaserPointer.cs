using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILaserPointer : MonoBehaviour
{
    [Header("References")]
    public Transform pointerOrigin;        // e.g. OVRCameraRig/TrackingSpace/RightHandAnchor
    public Camera uiEventCamera;           // usually the CenterEyeAnchor camera
    public LineRenderer line;              // assign a LineRenderer with 2 positions
    [Header("Config")]
    public float maxDistance = 8f;
    public LayerMask uiLayer = ~0;         // optional: filter world-space canvases
    public OVRInput.Button clickButton = OVRInput.Button.One;   // A (right) / X (left)
    public OVRInput.Controller controller = OVRInput.Controller.RTouch;

    // internal
    private PointerEventData _ped;
    private readonly List<RaycastResult> _results = new List<RaycastResult>();
    private GameObject _lastHover;

    void Awake()
    {
        if (uiEventCamera == null)
        {
            // Try to find the center eye camera automatically
            var cam = Camera.main;
            if (cam != null) uiEventCamera = cam;
        }
        if (line != null)
        {
            line.positionCount = 2;
            line.useWorldSpace = true;
        }
    }

    void Update()
    {
        if (pointerOrigin == null || EventSystem.current == null) return;

        // Build a UI ray from controller forward
        Vector3 origin = pointerOrigin.position;
        Vector3 dir = pointerOrigin.forward;

        // Create / reuse pointer event
        if (_ped == null) _ped = new PointerEventData(EventSystem.current);
        _ped.Reset();
        _ped.position = WorldRayToScreenPoint(uiEventCamera, origin, dir, maxDistance);

        // Raycast all UI
        _results.Clear();
        EventSystem.current.RaycastAll(_ped, _results);

        // Find first valid UI hit
        GameObject hitGO = null;
        Vector3 hitPoint = origin + dir * maxDistance;

        foreach (var r in _results)
        {
            // Optional: layer filter
            if (uiLayer == ~0 || ((1 << r.gameObject.layer) & uiLayer) != 0)
            {
                hitGO = r.gameObject;
                hitPoint = r.worldPosition != Vector3.zero ? r.worldPosition : hitPoint;
                break;
            }
        }

        // Draw laser
        if (line != null)
        {
            line.SetPosition(0, origin);
            line.SetPosition(1, hitGO ? hitPoint : origin + dir * maxDistance);
        }

        // Hover enter/exit
        if (hitGO != _lastHover)
        {
            if (_lastHover != null)
                ExecuteEvents.Execute(_lastHover, _ped, ExecuteEvents.pointerExitHandler);
            if (hitGO != null)
                ExecuteEvents.Execute(hitGO, _ped, ExecuteEvents.pointerEnterHandler);
            _lastHover = hitGO;
        }

        // Press / Click with A (or chosen button)
        if (OVRInput.GetDown(clickButton, controller))
        {
            if (hitGO != null)
                SendPressSequence(hitGO);
        }
        if (OVRInput.GetUp(clickButton, controller))
        {
            if (hitGO != null)
                SendReleaseAndClick(hitGO);
        }
    }

    // Convert world ray to a screen point along the ray for UI raycasters
    Vector2 WorldRayToScreenPoint(Camera cam, Vector3 origin, Vector3 dir, float dist)
    {
        if (cam == null)
        {
            // Fallback: project with a dummy
            var p = origin + dir * dist * 0.5f;
            return new Vector2(p.x, p.y);
        }
        var end = origin + dir * dist;
        Vector3 sp = cam.WorldToScreenPoint(end);
        return new Vector2(sp.x, sp.y);
    }

    void SendPressSequence(GameObject go)
    {
        _ped.pressPosition = _ped.position;
        _ped.pointerPressRaycast = _results.Count > 0 ? _results[0] : new RaycastResult();
        _ped.pointerPress = ExecuteEvents.ExecuteHierarchy(go, _ped, ExecuteEvents.pointerDownHandler);
        if (_ped.pointerPress == null)
        {
            // If no IPointerDownHandler, fallback to click handler target
            _ped.pointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(go);
        }
        _ped.rawPointerPress = go;
        ExecuteEvents.Execute(go, _ped, ExecuteEvents.beginDragHandler);
    }

    void SendReleaseAndClick(GameObject go)
    {
        ExecuteEvents.Execute(_ped.pointerPress, _ped, ExecuteEvents.pointerUpHandler);
        var clickHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(go);
        if (_ped.pointerPress == clickHandler)
        {
            ExecuteEvents.Execute(_ped.pointerPress, _ped, ExecuteEvents.pointerClickHandler);
        }
        ExecuteEvents.Execute(go, _ped, ExecuteEvents.endDragHandler);
        _ped.pointerPress = null;
        _ped.rawPointerPress = null;
    }
}
