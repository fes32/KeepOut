using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Chest : MonoBehaviour
{
    [SerializeField] private int _countSingTableInReward;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _openAudioClip;

    private bool _isOpen = false;
    private AudioSource _audioSource;

    public int CountSingTableReward => _countSingTableInReward;

    public event UnityAction<Chest> Opened;

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Open()
    {
        if (_isOpen == false)
        {
            Opened?.Invoke(this);
            _animator.Play("Open");
            _audioSource.PlayOneShot(_openAudioClip);
            _isOpen = true;
        }
    }
}