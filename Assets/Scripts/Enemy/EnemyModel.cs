using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyModel : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _takeDamage;
    [SerializeField] private AudioClip _attack;

    private AudioSource _audioSource;

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0.5f;
    }

    private void PlayAudio(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void TakeDamage()
    {
            _animator.Play("TakeDamage");

        PlayAudio(_takeDamage);
    }

    public void Attack()
    {
        _animator.Play("Attack");
    }

    public void PlayAttackSound()
    {
        PlayAudio(_attack);
    }

    public void Move()
    {
            _animator.Play("Move");
    }

    public void Idle()
    {
            _animator.Play("Idle");
    }
    public void Die()
    {
            _animator.Play("Die");
    }
}