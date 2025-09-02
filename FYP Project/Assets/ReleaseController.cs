using Oculus.Interaction.Input;
using Oculus.Interaction;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OVRGrabbable))]
public class ReleaseController : OVRGrabbable
{
    [Header("Optional settings")]
    public bool logDebug = true;

    public string EvidenceName;

    public GameObject Marker;

    // Primary source for reliable hand detection
    private IPointable _pointable; // from Grabbable (or other PointableElement)
    private DistanceGrabInteractable _distance; // for distance selection state

    // Track selection to avoid duplicate logs
    private readonly HashSet<int> _activeSelectors = new HashSet<int>();
    private bool _wasSelectedByDistance = false;

    // ---------- WIRING ----------

    private void Awake()
    {
        TryWireLocals(); // everything is on the same GO
    }

    private void OnEnable()
    {
        // In case enable order differs, try again
        if (_pointable == null || _distance == null)
            TryWireLocals();

        if (_pointable != null)
            _pointable.WhenPointerEventRaised += OnPointerEvent;
    }

    private void OnDisable()
    {
        if (_pointable != null)
            _pointable.WhenPointerEventRaised -= OnPointerEvent;

        _activeSelectors.Clear();
        _wasSelectedByDistance = false;
    }

    private void TryWireLocals()
    {
        // 1) We expect the *same GameObject* to hold Grabbable + DistanceGrabInteractable
        // First, grab the local Grabbable and use it as IPointable (this is where events fire)
        var localGrabbable = GetComponent<Grabbable>();
        if (localGrabbable != null)
        {
            _pointable = (IPointable)localGrabbable;
        }
        else
        {
            // fallback (shouldn’t be needed if your setup matches)
            _pointable = GetComponent<IPointable>();
        }

        // 2) Local DistanceGrabInteractable (for fallback state polling)
        _distance = GetComponent<DistanceGrabInteractable>();

        // Helpful log
        if (logDebug)
        {
            Debug.Log($"[ReleaseController] Wired Pointable={(_pointable as Component ? (_pointable as Component).name : "null")}  " +
                      $"Distance={(_distance ? _distance.name : "null")}", this);
        }
    }

    // ---------- RUNTIME (fallback for distance state only if we didn't get pointer events) ----------

    private void Update()
    {
        // If we couldn't hook a pointable (shouldn't happen with local Grabbable present),
        // use DistanceGrabInteractable.State as a fallback.
        if (_pointable == null && _distance != null)
        {
            var state = _distance.State; // InteractableState enum
            bool isSelected = state == InteractableState.Select;

            if (isSelected && !_wasSelectedByDistance)
            {
                _wasSelectedByDistance = true;
                var hand = GuessHandFromHierarchy(_distance.gameObject);
                OnGrab(hand);
            }
            else if (!isSelected && _wasSelectedByDistance)
            {
                _wasSelectedByDistance = false;
                OnRelease();
            }
        }
    }

    // ---------- POINTER EVENTS (preferred path, covers both near & distance grab) ----------

    private void OnPointerEvent(PointerEvent evt)
    {
        switch (evt.Type)
        {
            case PointerEventType.Select:
                {
                    bool wasEmpty = _activeSelectors.Count == 0;
                    _activeSelectors.Add(evt.Identifier);

                    var hand = DetectHandedness(evt);
                    if (wasEmpty)
                        OnGrab(hand);
                    break;
                }
            case PointerEventType.Unselect:
            case PointerEventType.Cancel:
                {
                    _activeSelectors.Remove(evt.Identifier);
                    if (_activeSelectors.Count == 0)
                        OnRelease();
                    break;
                }
        }
    }

    private Handedness DetectHandedness(PointerEvent evt)
    {
        var comp = evt.Data as Component;
        if (comp == null && evt.Data is GameObject goObj) comp = goObj.GetComponent<Component>();

        if (comp != null)
        {
            var ihand = comp.GetComponentInParent<IHand>();
            if (ihand != null) return ihand.Handedness;

            var handData = comp.GetComponentInParent<Hand>();
            if (handData != null) return handData.Handedness;

            // Name hints if no data components available
            string n = comp.gameObject.name.ToLower();
            if (n.Contains("left") || n.EndsWith("_l") || n.StartsWith("l_")) return Handedness.Left;
            if (n.Contains("right") || n.EndsWith("_r") || n.StartsWith("r_")) return Handedness.Right;
        }

        return Handedness.Right; // default if unknown
    }

    // Fallback attempt when we only know the interactable GameObject
    private Handedness GuessHandFromHierarchy(GameObject context)
    {
        var ihand = context.GetComponentInParent<IHand>();
        if (ihand != null) return ihand.Handedness;

        var handData = context.GetComponentInParent<Hand>();
        if (handData != null) return handData.Handedness;

        string n = context.name.ToLower();
        if (n.Contains("left")) return Handedness.Left;
        if (n.Contains("right")) return Handedness.Right;

        return Handedness.Right;
    }

    // ---------- YOUR CUSTOM CODE (kept exactly like you had) ----------
    public enum Handenum { Left, Right }
    [Header("Which hand triggers this pulse?")]
    public Handenum handd = Handenum.Right;
    private void OnGrab(Handedness hand)
    {
        //if (MetaHaptics.Instance == null) return;
        //var controller = (handd == Handenum.Left) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
        //MetaHaptics.Instance.Pulse(controller);

        if (OculusInput.GetRightGrip() > 0.8f) { var controllerr = OVRInput.Controller.RTouch; MetaHaptics.Instance.Pulse(controllerr); }
        else if (OculusInput.GetLeftGrip() > 0.8f) { var controllerrr = OVRInput.Controller.RTouch; MetaHaptics.Instance.Pulse(controllerrr); }
        SoundsManager.Instance.PlayPickSound();
        if (hand == Handedness.Left)
        {
            if (logDebug) Debug.Log($"{name} grabbed by LEFT hand");
            InputsController.Instance.Rightbag.SetActive(true);
        }
        else if (hand == Handedness.Right)
        {
            if (logDebug) Debug.Log($"{name} grabbed by RIGHT hand");
            InputsController.Instance.Leftbag.SetActive(true);
        }
        else
        {
            if (logDebug) Debug.Log($"{name} grabbed by UNKNOWN hand");
        }
        if(!IntroductionController.instance.interactedWithFirstGrabbable)
        {
            IntroductionController.instance.ShowhideIntroCanvas(true);
            IntroductionController.instance.interactedWithFirstGrabbable = true;
            IntroductionController.instance.HighlightObject.SetActive(false);
        }

    }

    private void OnRelease()
    {
        if (logDebug) Debug.Log($"{name} released");
        // If you want to hide both on release, uncomment:
        // InputsController.Instance.Leftbag.SetActive(false);
        // InputsController.Instance.Rightbag.SetActive(false);
    }
}
