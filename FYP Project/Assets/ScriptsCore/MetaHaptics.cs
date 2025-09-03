using System.Collections;
using UnityEngine;

public class MetaHaptics : MonoBehaviour
{
    public static MetaHaptics Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Pulse(OVRInput.Controller controller, float amplitude = 0.7f, float duration = 0.08f, float frequency = 1.0f)
    {
        StartCoroutine(PulseRoutine(controller, amplitude, duration, frequency));
    }

    private IEnumerator PulseRoutine(OVRInput.Controller controller, float amplitude, float duration, float frequency)
    {
        // For Quest controllers, frequency is often ignored; amplitude & duration matter most.
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(duration);
        OVRInput.SetControllerVibration(0f, 0f, controller);
    }
}
