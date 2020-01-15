using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{
    public AudioClips audioClips;

    [Range(3, 10)]
    [SerializeField] int audioSourcesAmount = 3;

    [SerializeField] GameObject audioSourceTemplate = null;
    [SerializeField] AudioMixer audioMixer = null;

    [SerializeField] private Settings settings = null;

    #region Audio settings

    private const string mixerMusicVolume = "MusicVolume", mixerSFXVolume = "SFXVolume";

    private const float mutedVolume = -80f;
    
    #endregion

    private List<AudioSource> audioSources;

    private AudioSource currentAudioSource;

    public AudioSource CurrentAudioSource { get { return currentAudioSource; } }

    protected override void OnAwake()
    {
        CreateAudioSources(audioSourcesAmount);
    }

    private void Start()
    {
        //In start because we depend on memento loading data process
        InitializeAudioMixer();
    }

    /// <summary>
    /// Initialize audiomixer
    /// </summary>
    private void InitializeAudioMixer()
    {
        if (!settings.isMusicActive)
        {
            audioMixer.SetFloat(mixerMusicVolume, mutedVolume);
        }

        if (!settings.isSFXActive)
        {
            audioMixer.SetFloat(mixerSFXVolume, mutedVolume);
        } 
    }

    /// <summary>
    /// Function that is called to mute audio mixer's channel
    /// </summary>
    /// <param name="_audioType"></param>
    public void MuteAudio(AudioType _audioType, Delegates.Action _onMutedAudio)
    {
        float value = 0f;

        switch (_audioType)
        {
            case AudioType.Music:
                audioMixer.GetFloat(mixerMusicVolume, out value);
                if (value > mutedVolume)
                {
                    settings.isMusicActive = false;
                    audioMixer.SetFloat(mixerMusicVolume, mutedVolume);
                }
                else if (value <= mutedVolume)
                {
                    settings.isMusicActive = true;
                    audioMixer.SetFloat(mixerMusicVolume, 0f);
                }
                break;
            case AudioType.SFX:
                audioMixer.GetFloat(mixerSFXVolume, out value);
                if (value > mutedVolume)
                {
                    audioMixer.SetFloat(mixerSFXVolume, mutedVolume);
                    settings.isSFXActive = false;
                }
                else if (value <= mutedVolume)
                {
                    settings.isSFXActive = true;
                    audioMixer.SetFloat(mixerSFXVolume, 0f);
                }
                break;
            default:
                break;
        }
        _onMutedAudio();
    }

    private void CreateAudioSources(int audioSourcesAmount)
    {
        audioSources = new List<AudioSource>();

        for (int i = 0; i < audioSourcesAmount; i++)
        {
            if (i == 0)//Music AudioSource
            {
                GameObject gameObject = Instantiate(audioSourceTemplate, transform);
                gameObject.name = string.Format("{0} AudioSource_{1}", AudioType.Music.ToString(), i);
                AudioSource audioSourceCreated = gameObject.GetComponent<AudioSource>();
                audioSourceCreated.outputAudioMixerGroup = audioMixer.FindMatchingGroups(AudioType.Music.ToString())[0];
                audioSourceCreated.loop = true;

                if (audioSourceCreated != null)
                {
                    audioSources.Add(audioSourceCreated);
                }
            }
            else// SFx AudioSources
            {
                GameObject gameObject = Instantiate(audioSourceTemplate, transform);
                gameObject.name = string.Format("{0} AudioSource_{1}", AudioType.SFX.ToString(), i);
                AudioSource audioSourceCreated = gameObject.GetComponent<AudioSource>();
                audioSourceCreated.outputAudioMixerGroup = audioMixer.FindMatchingGroups(AudioType.SFX.ToString())[0];

                if (audioSourceCreated != null)
                {
                    audioSources.Add(audioSourceCreated);
                }
            }
        }
    }

    /// <summary>
    /// Play one shot the _clipToPlay or loop it
    /// </summary>
    /// <param name="_clipToPlay"></param>
    /// <param name="_volume"></param>
    public void PlaySFx(AudioClip _clipToPlay, float _volume, bool _loop)
    {
        if (_loop)
        {
            if (!IsClipPlaying(_clipToPlay))
            {
                currentAudioSource = GetEmptyAudioSource(AudioType.SFX);

                if (currentAudioSource != null && _clipToPlay != null)
                {
                    currentAudioSource.clip = _clipToPlay;
                    currentAudioSource.volume = _volume;
                    currentAudioSource.loop = true;
                    currentAudioSource.Play();
                }
            }
        }
        else
        {
            currentAudioSource = GetAudioSource(AudioType.SFX);

            if (currentAudioSource != null && _clipToPlay != null)
            {
                currentAudioSource.PlayOneShot(_clipToPlay, _volume);
            }
        }
    }

    /// <summary>
    /// Time to Fade out will only works if there's another music playing.
    /// </summary>
    /// <param name="_clipToPlay"></param>
    /// <param name="_audioType"></param>
    /// <param name="_volume"></param>
    /// <param name="_timeToFadeOut"></param>
    /// <param name="_timeToFadeIn"></param>
    public void PlayMusic(AudioClip _clipToPlay, float _volume, float _timeToFadeOut, float _timeToFadeIn)
    {
        currentAudioSource = GetAudioSource(AudioType.Music);

        if (currentAudioSource != null && _clipToPlay != null)
        {
            if (!currentAudioSource.isPlaying)
            {
                currentAudioSource.clip = _clipToPlay;
                StartCoroutine(MusicTrack(currentAudioSource, _volume , _timeToFadeIn));
            }
            else
            {
                StartCoroutine(ChangeMusicTracks(currentAudioSource, _clipToPlay, _volume, _timeToFadeOut, _timeToFadeIn));
            }
        }
    }

    private IEnumerator MusicTrack(AudioSource _currentAudioSource, float _volume, float _timeToFadeIn)
    {
        _currentAudioSource.volume = 0f;
        _currentAudioSource.Play();

        float elapsedTime = 0f;
        float currentVolume = _currentAudioSource.volume;

        while (elapsedTime < _timeToFadeIn)
        {
            _currentAudioSource.volume = Mathf.Lerp(currentVolume, _volume, elapsedTime / _timeToFadeIn);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _currentAudioSource.volume = _volume;
    }
    private IEnumerator ChangeMusicTracks(AudioSource _currentAudioSource, AudioClip _newMusicTrack, float _volume, float _timeToFadeOut, float _timeToFadeIn)
    {
        float elapsedTime = 0f;
        float currentVolume = _currentAudioSource.volume;

        while (elapsedTime < _timeToFadeOut)
        {
            _currentAudioSource.volume = Mathf.Lerp(currentVolume, 0f, elapsedTime / _timeToFadeOut);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _currentAudioSource.clip = _newMusicTrack;

        currentVolume = 0f;
        elapsedTime = 0f;

        while (elapsedTime < _timeToFadeIn)
        {
            _currentAudioSource.volume = Mathf.Lerp(currentVolume, _volume, elapsedTime / _timeToFadeIn);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _currentAudioSource.volume = _volume;
    }

    private AudioSource GetAudioSource(AudioType _audioType)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            switch (_audioType) 
            {
                case AudioType.Music:
                    if (audioSources[i].outputAudioMixerGroup == audioMixer.FindMatchingGroups(_audioType.ToString())[0])
                    {
                        return audioSources[i];
                    }
                    break;
                case AudioType.SFX:
                    if (audioSources[i].volume == 1 && audioSources[i].pitch == 1)
                    {
                        if (audioSources[i].outputAudioMixerGroup == audioMixer.FindMatchingGroups(_audioType.ToString())[0])
                        {
                            return audioSources[i];
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        GameObject gameObject = Instantiate(audioSourceTemplate);
        gameObject.name = string.Format("{0} AudioSource_{1}", _audioType.ToString(), audioSources.Count);
        AudioSource audioSourceCreated = gameObject.GetComponent<AudioSource>();
        audioSourceCreated.outputAudioMixerGroup = audioMixer.FindMatchingGroups(_audioType.ToString())[0];

        if (audioSourceCreated != null)
        {
            audioSources.Add(audioSourceCreated);
        }

        return audioSourceCreated;
    }
    private AudioSource GetEmptyAudioSource(AudioType _audioType)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            switch (_audioType)
            {
                case AudioType.Music:
                    if (audioSources[i].clip == null)
                    {
                        if (audioSources[i].outputAudioMixerGroup == audioMixer.FindMatchingGroups(_audioType.ToString())[0])
                        {
                            return audioSources[i];
                        } 
                    }
                    break;
                case AudioType.SFX:
                    if (audioSources[i].volume == 1 && audioSources[i].pitch == 1)
                    {
                        if (audioSources[i].clip == null)
                        {
                            if (audioSources[i].outputAudioMixerGroup == audioMixer.FindMatchingGroups(_audioType.ToString())[0])
                            {
                                return audioSources[i];
                            } 
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        GameObject gameObject = Instantiate(audioSourceTemplate);
        gameObject.name = string.Format("{0} AudioSource_{1}", _audioType.ToString(), audioSources.Count);
        AudioSource audioSourceCreated = gameObject.GetComponent<AudioSource>();
        audioSourceCreated.outputAudioMixerGroup = audioMixer.FindMatchingGroups(_audioType.ToString())[0];

        if (audioSourceCreated != null)
        {
            audioSources.Add(audioSourceCreated);
        }

        return audioSourceCreated;
    }

    /// <summary>
    /// Reset and stop the audiosource
    /// </summary>
    /// <param name="_clipToStop">Clip to find the AudioSource that will be reset</param>
    public void StopByClip(AudioClip _clipToStop)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].clip == _clipToStop)
            {
                audioSources[i].Stop();
                audioSources[i].clip = null;
                audioSources[i].loop = false;
                audioSources[i].volume = 1f;
                audioSources[i].pitch = 1f;
                return;
            }
        }
    }

    private bool IsClipPlaying(AudioClip _clipToSearch)
    {
        bool result = false;

        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].clip == _clipToSearch)
            {
                return result = true;
            }
        }

        return result;
    }
}