using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager Instance;

    public AudioSource MusicSource;
    public AudioSource SoundsSource;

    public AudioClip PickSound;
    public AudioClip DropSound;

    public void PlayPickSound()
    {
        SoundsSource.clip = PickSound;
        SoundsSource.Play();
    }

    public void PlayDropSound()
    {
        SoundsSource.clip = DropSound;
        SoundsSource.Play();
    }
    private void Awake()
    {
        Instance = this;
    }
}
