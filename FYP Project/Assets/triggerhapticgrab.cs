using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine;
using Oculus.Haptics;

public class TriggerHaptic : MonoBehaviour
{
    [Range(0, 2.5f)]
    public float duration = 1f;  // Haptic duration

    [Range(0, 1)]
    public float amplitude = 1f; // Haptic amplitude

    [Range(0, 1)]
    public float frequency = 0.5f; // Haptic frequency

    public GrabInteractable grabInteractable;

    private void Start()
    {
        // Subscribe to the "WhenSelectingInteractorAdded" event
        grabInteractable.WhenSelectingInteractorAdded.Action += WhenSelectingInteractorAdded_Action;
    }

    private void WhenSelectingInteractorAdded_Action(GrabInteractor obj)
    {
        // Check which hand grabbed the object
        ControllerRef controllerRef = obj.GetComponent<ControllerRef>();
        if (controllerRef)
        {
            if (controllerRef.Handedness == Handedness.Right)
            {
                TriggerHaptics(OVRInput.Controller.RTouch);
            }
            else
            {
                TriggerHaptics(OVRInput.Controller.LTouch);
            }
        }
    }

    public void TriggerHaptics(OVRInput.Controller controller)
    {
        // Start the haptics coroutine
        StartCoroutine(TriggerHapticsRoutine(controller));
    }

    private IEnumerator TriggerHapticsRoutine(OVRInput.Controller controller)
    {
        // Turn on haptics
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(duration);
        // Turn off 
    }
}