using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayController : MonoBehaviour
{
    [Header("Scene References")]
    [Tooltip("Renderer to toggle on/off when hovering UI. If null, will try GetComponent<MeshRenderer>().")]
    public MeshRenderer rayRenderer;

    [Header("Layers")]
    [Tooltip("Layer considered as UI (must match your UI objects' layer).")]
    public LayerMask uiLayer; // set this to the UI layer in the Inspector

    [Header("Input")]
    [Tooltip("Keyboard fallback for Primary Press (useful in Editor).")]
    public KeyCode primaryPressKey = KeyCode.Space;

#if OCULUS_PRESENT
    [Tooltip("Use Oculus Integration's OVRInput for Primary Press (optional).")]
    public bool useOVRInput = true;
    public OVRInput.Button ovrPrimaryButton = OVRInput.Button.PrimaryIndexTrigger; // or Button.One (A/X)
    public OVRInput.Controller ovrController = OVRInput.Controller.RTouch;        // choose hand
#else
    // When Oculus Integration isn't present, toggle in Inspector has no effect.
    private readonly bool useOVRInput = false;
#endif

    // Track what we're overlapping
    private int uiOverlapCount = 0;
    private readonly HashSet<Collider> activeUIColliders = new HashSet<Collider>();
    private readonly List<Button> hoveredButtons = new List<Button>(); // simple stack: last entered is current

    private void Awake()
    {
        if (rayRenderer == null) rayRenderer = GetComponent<MeshRenderer>();
        SetRayVisible(false);

        // Ensure our collider is a trigger
        var col = GetComponent<Collider>();
        if (col && !col.isTrigger)
        {
            Debug.LogWarning("[RayController] Ray collider should be set to IsTrigger. Fixing at runtime.");
            col.isTrigger = true;
        }
    }
    [Header("Click Settings")]
    public float triggerDownThreshold = 0.8f;  // fire when >= this
    public float triggerUpThreshold = 0.4f;  // consider released when <= this (hysteresis)
    public float clickCooldown = 0.25f; // seconds between allowed clicks

    private bool triggerIsDown = false;       // edge detection state
    private float lastClickTime = -999f;       // cooldown timer

    private void TryClickOnce()
    {
        if (buttonInRay == null) return;
        if (!buttonInRay.interactable) return;

        // cooldown guard
        if (Time.time - lastClickTime < clickCooldown) return;

        var img = buttonInRay.GetComponent<Image>();
        if (img != null) img.color = Color.grey;

        buttonInRay.onClick.Invoke();
        lastClickTime = Time.time;
    }
    private void Update()
    {
        // Handle primary press input
        //if (PrimaryPressDown())
        //{
        //    var btn = GetCurrentButton();
        //    if (btn != null && btn.interactable)
        //    {
        //        // Optionally: give tiny visual feedback before invoking
        //        // Debug.Log($"Invoking onClick of {btn.name}");
        //        btn.onClick.Invoke();
        //    }
        //}
        //if (OculusInput.GetLeftTrigger() > 0.8f && buttonInRay == null)
        //{
        //    InputsController.Instance.logger.text = "button in ray is null";
            
        //}

        //if (OculusInput.GetButtonDownX() && buttonInRay == null)
        //{
        //    InputsController.Instance.logger.text = "button in ray is null with x button";
        //}

        //if (OculusInput.GetLeftTrigger() > 0.8f && buttonInRay != null)
        //{
        //    buttonInRay.GetComponent<Image>().color = Color.grey;
        //    buttonInRay.onClick.Invoke();
        //}

        //if(OculusInput.GetButtonDownX() && buttonInRay != null)
        //{
        //    buttonInRay.GetComponent<Image>().color = Color.grey;
        //    buttonInRay.onClick.Invoke();
        //}

        // ---- Analog trigger edge detection ----
        float lt = OculusInput.GetLeftTrigger();

        // rising edge (just pressed)
        if (!triggerIsDown && lt >= triggerDownThreshold)
        {
            triggerIsDown = true;
            TryClickOnce();  // fires at most once, respects cooldown
        }
        // falling edge (released enough)
        else if (triggerIsDown && lt <= triggerUpThreshold)
        {
            triggerIsDown = false;
        }

        // ---- Discrete button (already GetButtonDown) ----
        if (OculusInput.GetButtonDownX())
        {
            TryClickOnce();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOnUILayer(other.gameObject)) return;
        if (other.gameObject.GetComponent<Button>() != null) { other.gameObject.GetComponent<Image>().color = Color.grey; buttonInRay = other.gameObject.GetComponent<Button>(); }
        if (!activeUIColliders.Contains(other))
        {
            activeUIColliders.Add(other);
            uiOverlapCount++;
            if (uiOverlapCount == 1) SetRayVisible(true);
        }

        // If this collider (or its relatives) has a Button, track it
        var button = FindButtonOnHierarchy(other.transform);
        if (button != null && !hoveredButtons.Contains(button))
        {
            hoveredButtons.Add(button);
            // Optional: highlight could be added here (e.g., change colors)
            // var colors = button.colors; colors.normalColor = ...; button.colors = colors;
        }
    }

   
    Button buttonInRay = null;
    private void OnTriggerExit(Collider other)
    {
        if (!IsOnUILayer(other.gameObject)) return;

        if (other.gameObject.GetComponent<Button>() != null) { other.gameObject.GetComponent<Image>().color = Color.white; buttonInRay = null; }

        if (activeUIColliders.Remove(other))
        {
            uiOverlapCount = Mathf.Max(0, uiOverlapCount - 1);
            if (uiOverlapCount == 0) SetRayVisible(false);
        }

        // If we were tracking a button from this collider, try to remove it
        var button = FindButtonOnHierarchy(other.transform);
        if (button != null)
        {
            hoveredButtons.Remove(button);
        }
        else
        {
            // In case collider belonged to a child without a Button component,
            // ensure we drop any buttons that are no longer overlapped.
            CleanupStaleButtons();
        }
    }

    // ---------- Helpers ----------

    private void SetRayVisible(bool visible)
    {
        if (rayRenderer != null) rayRenderer.enabled = visible;
    }

    private bool IsOnUILayer(GameObject go)
    {
        return ((1 << go.layer) & uiLayer.value) != 0;
    }

    private Button FindButtonOnHierarchy(Transform t)
    {
        if (t == null) return null;

        // Try self, then parents, then children (covers common setups)
        var btn = t.GetComponent<Button>();
        if (btn != null) return btn;

        btn = t.GetComponentInParent<Button>();
        if (btn != null) return btn;

        return t.GetComponentInChildren<Button>();
    }

    private Button GetCurrentButton()
    {
        // Return the most recently hovered (top of stack)
        for (int i = hoveredButtons.Count - 1; i >= 0; i--)
        {
            if (hoveredButtons[i] != null) return hoveredButtons[i];
        }
        return null;
    }

    private void CleanupStaleButtons()
    {
        // Remove nulls or buttons whose colliders are no longer overlapping
        for (int i = hoveredButtons.Count - 1; i >= 0; i--)
        {
            var b = hoveredButtons[i];
            if (b == null)
            {
                hoveredButtons.RemoveAt(i);
                continue;
            }
            // If button has any collider still in activeUIColliders, keep it
            bool stillOverlapping = false;
            foreach (var col in b.GetComponentsInChildren<Collider>())
            {
                if (col != null && activeUIColliders.Contains(col))
                {
                    stillOverlapping = true;
                    break;
                }
            }
            if (!stillOverlapping)
                hoveredButtons.RemoveAt(i);
        }
    }

    private bool PrimaryPressDown()
    {
#if OCULUS_PRESENT
        if (useOVRInput && OVRInput.IsControllerConnected(ovrController))
        {
            return OVRInput.GetDown(ovrPrimaryButton, ovrController);
        }
#endif
        // Keyboard fallback (useful for editor testing)
        return Input.GetKeyDown(primaryPressKey);
    }
}
