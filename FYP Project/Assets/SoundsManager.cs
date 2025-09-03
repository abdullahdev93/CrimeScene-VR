using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager Instance;

    public AudioSource MusicSource;
    public AudioSource SoundsSource;

    public AudioClip PickSound;
    public AudioClip DropSound;

    public AudioSource PressXtoMovein;
    public AudioSource MoveIntoRoom;
    public AudioSource IntroSource;
    public AudioSource CollectFirstEvidence;
    public AudioSource CollectFirstEvidenceCompleted;
    public AudioSource AllEvidenceCompleted;
    public AudioSource PickOneCriminal;
    public AudioSource TeleportSound;
    public AudioSource UIClickSound;
    public AudioSource CorrectProfileSelected;
    public AudioSource RoomEnttryDeniedSound;
    public AudioSource WornProfileSelected;

    private void Awake()
    {
        Instance = this;
    }

    // ---------- Public Play Methods ----------
    public void PlayPickSound()
    {
        //StopAllSources();
        SoundsSource.clip = PickSound;
        SoundsSource.Play();
    }

    public void PlayDropSound()
    {
        //StopAllSources();
        SoundsSource.clip = DropSound;
        SoundsSource.Play();
    }

    public void PlayPressXtoMovein()
    {
        StopAllSources();
        PressXtoMovein?.Play();
    }

    public void PlayMoveIntoRoom()
    {
        StopAllSources();
        MoveIntoRoom?.Play();
    }

    public void PlayIntroSource()
    {
        StopAllSources();
        IntroSource?.Play();
    }

    public void PlayCollectFirstEvidence()
    {
        StopAllSources();
        CollectFirstEvidence?.Play();
    }

    public void PlayCollectFirstEvidenceCompleted()
    {
        StopAllSources();
        CollectFirstEvidenceCompleted?.Play();
    }

    public void PlayAllEvidenceCompleted()
    {
        StopAllSources();
        AllEvidenceCompleted?.Play();
    }

    public void PlayPickOneCriminal()
    {
        StopAllSources();
        PickOneCriminal?.Play();
    }

    public void PlayTeleportSound()
    {
        StopAllSources();
        TeleportSound?.Play();
    }

    public void PlayUIClickSound()
    {
        //StopAllSources();
        UIClickSound?.Play();
    }

    public void PlayCorrectProfileSelected()
    {
        StopAllSources();
        CorrectProfileSelected?.Play();
    }

    public void PlayRoomEnttryDeniedSound()
    {
        StopAllSources();
        RoomEnttryDeniedSound?.Play();
    }

    public void PlayWornProfileSelected()
    {
        StopAllSources();
        WornProfileSelected?.Play();
    }

    // ---------- Stop All ----------
    public void StopAllSources()
    {
        PressXtoMovein?.Stop();
        MoveIntoRoom?.Stop();
        IntroSource?.Stop();
        CollectFirstEvidence?.Stop();
        CollectFirstEvidenceCompleted?.Stop();
        AllEvidenceCompleted?.Stop();
        PickOneCriminal?.Stop();
        TeleportSound?.Stop();
        UIClickSound?.Stop();
        CorrectProfileSelected?.Stop();
        RoomEnttryDeniedSound?.Stop();
        WornProfileSelected?.Stop();

        // Stop any extra suspect UI sounds
        if (SuspectBoardUIHandler.instance != null)
            SuspectBoardUIHandler.instance.StopAllSuspectSources();
    }
}
