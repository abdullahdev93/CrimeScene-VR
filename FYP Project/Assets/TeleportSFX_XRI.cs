// TeleportSFX_XRI.cs
using Oculus.Interaction.Locomotion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportSFX_XRI : MonoBehaviour
{
    [Tooltip("Reference to the TeleportInteractor in your scene (usually on the hand/controller).")]
    public TeleportInteractor teleportInteractor;

    [Tooltip("Sound clip to play when teleport completes.")]
    public AudioClip teleportClip;

    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _audio.playOnAwake = false;
        if (teleportInteractor == null)
        {
            teleportInteractor = FindObjectOfType<TeleportInteractor>();
        }
    }

    private void OnEnable()
    {
        if (teleportInteractor != null)
        {
            teleportInteractor.WhenLocomotionPerformed += OnTeleportPerformed;
        }
    }

    private void OnDisable()
    {
        if (teleportInteractor != null)
        {
            teleportInteractor.WhenLocomotionPerformed -= OnTeleportPerformed;
        }
    }

    private void OnTeleportPerformed(LocomotionEvent locomotionEvent)
    {
        if (teleportClip != null && _audio != null)
        {
            _audio.PlayOneShot(teleportClip);
        }
    }
}
