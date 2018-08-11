using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAudio : AudioBase {

    private void Awake()
    {
        Bind(AudioEvent.PLAY_BG_AUDIO,
            AudioEvent.STOP_BG_AUDIO,
            AudioEvent.PLAY_BG_VOLUME);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case AudioEvent.PLAY_BG_AUDIO:
                playAudio();
                break;
            case AudioEvent.STOP_BG_AUDIO:
                stopAudio();
                break;
            case AudioEvent.PLAY_BG_VOLUME:
                setVolume((float)message);
                break;
            default:
                break;
        }
    }

    [SerializeField]
    private AudioSource audioSource;

    private void playAudio()
    {
        audioSource.Play();
    }

    private void setVolume(float value)
    {
        audioSource.volume = value;
    }

    private void stopAudio()
    {
        audioSource.Stop();
    }
}
