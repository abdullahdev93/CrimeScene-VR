using UnityEngine;
using UnityEngine.Events;

#if UNITY_ANDROID || UNITY_STANDALONE || UNITY_EDITOR
// OVRInput is provided by the Meta/Oculus Integration.
#endif

/// <summary>
/// Centralized handler for Oculus Quest controllers using OVRInput (Meta XR / Oculus Integration).
/// - Raises UnityEvents for press/release/touch for A/B/X/Y, thumbstick clicks, menu, triggers, grips.
/// - Provides per-hand thumbstick vectors and analog trigger/grip values (with deadzone).
/// - Example: toggles canvases on A (right) and B (right).
/// </summary>
public class OculusInputController : MonoBehaviour
{
    [Header("Optional UI (Example)")]
    [Tooltip("Shown/hidden when RIGHT A is pressed/released")]
    public Canvas rightA_Canvas;
    [Tooltip("Shown/hidden when RIGHT B is pressed/released")]
    public Canvas rightB_Canvas;

    [Header("Thumbstick Deadzone")]
    [Range(0f, 0.5f)] public float stickDeadzone = 0.15f;

    [Header("Trigger Deadzone")]
    [Range(0f, 0.4f)] public float triggerDeadzone = 0.05f;

    [System.Serializable] public class ButtonEvent : UnityEvent { }
    [System.Serializable] public class Axis1DEvent : UnityEvent<float> { }
    [System.Serializable] public class Axis2DEvent : UnityEvent<Vector2> { }

    // ===== RIGHT HAND EVENTS =====
    [Header("Right Hand - Buttons")]
    public ButtonEvent OnRightA_Pressed;
    public ButtonEvent OnRightA_Released;
    public ButtonEvent OnRightB_Pressed;
    public ButtonEvent OnRightB_Released;
    public ButtonEvent OnRightThumbstickClick_Pressed;
    public ButtonEvent OnRightThumbstickClick_Released;
    public ButtonEvent OnRightMenu_Pressed;     // Start/Menu (if available)
    public ButtonEvent OnRightMenu_Released;

    [Header("Right Hand - Touch")]
    public ButtonEvent OnRightA_TouchDown;
    public ButtonEvent OnRightA_TouchUp;
    public ButtonEvent OnRightB_TouchDown;
    public ButtonEvent OnRightB_TouchUp;
    public ButtonEvent OnRightThumbRest_TouchDown;
    public ButtonEvent OnRightThumbRest_TouchUp;

    [Header("Right Hand - Analog")]
    public Axis2DEvent OnRightThumbstick;
    public Axis1DEvent OnRightIndexTrigger;
    public Axis1DEvent OnRightGrip;

    // ===== LEFT HAND EVENTS =====
    [Header("Left Hand - Buttons")]
    public ButtonEvent OnLeftX_Pressed;
    public ButtonEvent OnLeftX_Released;
    public ButtonEvent OnLeftY_Pressed;
    public ButtonEvent OnLeftY_Released;
    public ButtonEvent OnLeftThumbstickClick_Pressed;
    public ButtonEvent OnLeftThumbstickClick_Released;

    [Header("Left Hand - Touch")]
    public ButtonEvent OnLeftX_TouchDown;
    public ButtonEvent OnLeftX_TouchUp;
    public ButtonEvent OnLeftY_TouchDown;
    public ButtonEvent OnLeftY_TouchUp;
    public ButtonEvent OnLeftThumbRest_TouchDown;
    public ButtonEvent OnLeftThumbRest_TouchUp;

    [Header("Left Hand - Analog")]
    public Axis2DEvent OnLeftThumbstick;
    public Axis1DEvent OnLeftIndexTrigger;
    public Axis1DEvent OnLeftGrip;

    // ===== Internal state tracking for edge detection =====
    bool rA_prev, rB_prev, rStick_prev, rMenu_prev;
    bool lX_prev, lY_prev, lStick_prev;

    bool rA_touch_prev, rB_touch_prev, rThumbRest_touch_prev;
    bool lX_touch_prev, lY_touch_prev, lThumbRest_touch_prev;

    void Awake()
    {
        // Optional: ensure canvases start hidden
        if (rightA_Canvas) rightA_Canvas.enabled = false;
        if (rightB_Canvas) rightB_Canvas.enabled = false;
    }

    void Update()
    {
        // -------- BUTTON PRESSES (edge-based) --------
        // A/B are RIGHT, X/Y are LEFT in typical Oculus mapping via OVRInput.Button.One/Two with controller specificity.

        // RIGHT: A (One on RTouch)
        EdgeButton(OVRInput.Button.One, OVRInput.Controller.RTouch,
            ref rA_prev, OnRightA_Pressed, OnRightA_Released,
            onPress: () => { if (rightA_Canvas) rightA_Canvas.enabled = true; },
            onRelease: () => { if (rightA_Canvas) rightA_Canvas.enabled = false; });

        // RIGHT: B (Two on RTouch)
        EdgeButton(OVRInput.Button.Two, OVRInput.Controller.RTouch,
            ref rB_prev, OnRightB_Pressed, OnRightB_Released,
            onPress: () => { if (rightB_Canvas) rightB_Canvas.enabled = true; },
            onRelease: () => { if (rightB_Canvas) rightB_Canvas.enabled = false; });

        // RIGHT: Thumbstick Click
        EdgeButton(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch,
            ref rStick_prev, OnRightThumbstickClick_Pressed, OnRightThumbstickClick_Released);

        // RIGHT: Menu/Start (if allowed; some system buttons are reserved)
        EdgeButton(OVRInput.Button.Start, OVRInput.Controller.RTouch,
            ref rMenu_prev, OnRightMenu_Pressed, OnRightMenu_Released);

        // LEFT: X (One on LTouch)
        EdgeButton(OVRInput.Button.One, OVRInput.Controller.LTouch,
            ref lX_prev, OnLeftX_Pressed, OnLeftX_Released);

        // LEFT: Y (Two on LTouch)
        EdgeButton(OVRInput.Button.Two, OVRInput.Controller.LTouch,
            ref lY_prev, OnLeftY_Pressed, OnLeftY_Released);

        // LEFT: Thumbstick Click
        EdgeButton(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch,
            ref lStick_prev, OnLeftThumbstickClick_Pressed, OnLeftThumbstickClick_Released);

        // -------- TOUCH STATES (edge-based) --------
        // RIGHT touches
        EdgeTouch(OVRInput.Touch.One, OVRInput.Controller.RTouch, ref rA_touch_prev, OnRightA_TouchDown, OnRightA_TouchUp);
        EdgeTouch(OVRInput.Touch.Two, OVRInput.Controller.RTouch, ref rB_touch_prev, OnRightB_TouchDown, OnRightB_TouchUp);
        EdgeTouch(OVRInput.Touch.PrimaryThumbRest, OVRInput.Controller.RTouch, ref rThumbRest_touch_prev, OnRightThumbRest_TouchDown, OnRightThumbRest_TouchUp);

        // LEFT touches
        EdgeTouch(OVRInput.Touch.One, OVRInput.Controller.LTouch, ref lX_touch_prev, OnLeftX_TouchDown, OnLeftX_TouchUp);
        EdgeTouch(OVRInput.Touch.Two, OVRInput.Controller.LTouch, ref lY_touch_prev, OnLeftY_TouchDown, OnLeftY_TouchUp);
        EdgeTouch(OVRInput.Touch.PrimaryThumbRest, OVRInput.Controller.LTouch, ref lThumbRest_touch_prev, OnLeftThumbRest_TouchDown, OnLeftThumbRest_TouchUp);

        // -------- ANALOG / AXES (continuous) --------
        // Thumbsticks (Vector2), with deadzone
        Vector2 rStick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        Vector2 lStick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

        if (rStick.magnitude >= stickDeadzone) OnRightThumbstick?.Invoke(rStick);
        if (lStick.magnitude >= stickDeadzone) OnLeftThumbstick?.Invoke(lStick);

        // Index triggers (0..1)
        float rIndex = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        float lIndex = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        if (rIndex >= triggerDeadzone) OnRightIndexTrigger?.Invoke(rIndex);
        if (lIndex >= triggerDeadzone) OnLeftIndexTrigger?.Invoke(lIndex);

        // Grip (hand) triggers (0..1)
        float rGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        float lGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        if (rGrip >= triggerDeadzone) OnRightGrip?.Invoke(rGrip);
        if (lGrip >= triggerDeadzone) OnLeftGrip?.Invoke(lGrip);
    }

    // ===== Helper: button press/release edges for a specific controller =====
    void EdgeButton(OVRInput.Button button, OVRInput.Controller controller,
                    ref bool prevState, ButtonEvent onDown, ButtonEvent onUp,
                    System.Action onPress = null, System.Action onRelease = null)
    {
        bool now = OVRInput.Get(button, controller);
        if (now && !prevState)
        {
            prevState = true;
            onPress?.Invoke();
            onDown?.Invoke();
        }
        else if (!now && prevState)
        {
            prevState = false;
            onRelease?.Invoke();
            onUp?.Invoke();
        }
    }

    // ===== Helper: touch down/up edges for a specific controller =====
    void EdgeTouch(OVRInput.Touch touch, OVRInput.Controller controller,
                   ref bool prevState, ButtonEvent onDown, ButtonEvent onUp)
    {
        bool now = OVRInput.Get(touch, controller);
        if (now && !prevState)
        {
            prevState = true;
            onDown?.Invoke();
        }
        else if (!now && prevState)
        {
            prevState = false;
            onUp?.Invoke();
        }
    }

    // ===== Public convenience methods (manual calls if you prefer code over UnityEvents) =====

    // RIGHT — A / B
    public void ShowRightA() { if (rightA_Canvas) rightA_Canvas.enabled = true; }
    public void HideRightA() { if (rightA_Canvas) rightA_Canvas.enabled = false; }
    public void ShowRightB() { if (rightB_Canvas) rightB_Canvas.enabled = true; }
    public void HideRightB() { if (rightB_Canvas) rightB_Canvas.enabled = false; }

    // Example getters if you want to poll from other scripts:
    public Vector2 GetRightThumbstick() =>
        OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
    public Vector2 GetLeftThumbstick() =>
        OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
    public float GetRightIndexTrigger() =>
        OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
    public float GetLeftIndexTrigger() =>
        OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
    public float GetRightGrip() =>
        OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);
    public float GetLeftGrip() =>
        OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
}
