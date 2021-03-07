using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MenuAudioSource : MonoBehaviour
{
    [SerializeField] private AudioClip _backGroundMusic;

    private AudioSource _audioSource;

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _backGroundMusic;
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
