using UnityEngine;

public class MusicInitialization : MonoBehaviour
{
    private void Start()
    {
        //This is in start because it depends on the audio manager awake initialization.
        AudioManager.Instance.PlayMusic(AudioManager.Instance.audioClips.inGameMusic, 0.6f, 1f, 2f);
    }
}
