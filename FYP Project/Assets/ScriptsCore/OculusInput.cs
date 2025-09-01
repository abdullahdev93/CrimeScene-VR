using UnityEngine;
using Oculus.Platform; // only if you use platform SDK, safe to remove

/// <summary>
/// Wrapper around OVRInput to mimic Unity's Input.GetButton style.
/// Call these methods from any script without needing references.
/// </summary>
public static class OculusInput
{
    // === Buttons ===
    public static bool GetButtonA() => OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch);
    public static bool GetButtonB() => OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch);
    public static bool GetButtonX() => OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.LTouch);
    public static bool GetButtonY() => OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.LTouch);

    public static bool GetRightThumbstickClick() => OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch);
    public static bool GetLeftThumbstickClick() => OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch);

    public static bool GetMenuButton() => OVRInput.Get(OVRInput.Button.Start, OVRInput.Controller.RTouch);

    // === Button Down / Up ===
    public static bool GetButtonDownA() => OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch);
    public static bool GetButtonUpA() => OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch);

    public static bool GetButtonDownB() => OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch);
    public static bool GetButtonUpB() => OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch);

    public static bool GetButtonDownX() => OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch);
    public static bool GetButtonUpX() => OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.LTouch);

    public static bool GetButtonDownY() => OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch);
    public static bool GetButtonUpY() => OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.LTouch);

    // === Thumbsticks (axes) ===
    public static Vector2 GetRightThumbstick() =>
        OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

    public static Vector2 GetLeftThumbstick() =>
        OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

    // === Triggers & Grips (0..1) ===
    public static float GetRightTrigger() =>
        OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

    public static float GetLeftTrigger() =>
        OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);

    public static float GetRightGrip() =>
        OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);

    public static float GetLeftGrip() =>
        OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
}
