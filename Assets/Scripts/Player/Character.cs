using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _attack;
    [SerializeField] private AudioClip _setTable;
    [SerializeField] private AudioClip _useHealthPotion;

    private void PlayAudio(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private void PlayAudioWithDelay(AudioClip clip, float delay)
    {
        StartCoroutine(WaitTimeForPlayingAudio(clip, delay));
    }

    private IEnumerator WaitTimeForPlayingAudio(AudioClip clip, float delay)
    {
        float elapsedTime = 0;

        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void Move()
    {
       _animator.Play("Walk");
    }

    public void Attack()
    {
        _animator.Play("Attack");
        PlayAudioWithDelay(_attack,0.7f);
    }

    public void Idle()
    {
        _animator.Play("Idle");
    }

    public void Healing()
    {
        PlayAudio(_useHealthPotion);
    }

    public void SetTable()
    {
        PlayAudio(_setTable);
    }
}