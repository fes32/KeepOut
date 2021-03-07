using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _openAudioClip;

    private bool _isOpen = false;
    private AudioSource _audioSource;

    public event UnityAction Opened;

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (_isOpen == false)
            {
                Open();
            }
        }
    }

    private void Open()
    {
        Opened?.Invoke();
        _animator.Play("Open");
        _audioSource.PlayOneShot(_openAudioClip);
        _isOpen = true;
    }
}