using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackGroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip[] _musicTracks;
    [SerializeField] private Player _player;
    [SerializeField] private PausePanel _pausePanel;

    private AudioSource _audioSource;
    private AudioClip _currentMusicTrack;

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
        _player.Dying += PlayerDied;

        _pausePanel.MusicOff += Pause;
        _pausePanel.MusicOn += PlayAfterPause;
        
        Play();
    }

    private void OnDisable()
    {
        _player.Dying -= PlayerDied;

        _pausePanel.MusicOff -= Pause;
        _pausePanel.MusicOn -= PlayAfterPause;
    }

    private IEnumerator ListenCurrentMusic()
    {
        float musicTrackDuration = _currentMusicTrack.length;
        float elapsedTime = 0;

        while (elapsedTime < musicTrackDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Play();
    }

    private void Play()
    {
        _currentMusicTrack = GetRandomNextMusicTrack(); 

        _audioSource.clip = _currentMusicTrack;
        _audioSource.Play();
        StartCoroutine(ListenCurrentMusic());
    }

    private void PlayAfterPause()
    {
        if (_audioSource.clip != null)
            _audioSource.Play(); 
    }

    private void Pause()
    {
        _audioSource.Pause();
    }

    private AudioClip GetRandomNextMusicTrack()
    {
        int nextMusicTrack = Random.Range(0, _musicTracks.Length);

        return _musicTracks[nextMusicTrack];
    }

    private void PlayerDied()
    {
        Pause();
    }
}
