using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : DropItem
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private int _damage;
    [SerializeField] private float _damageRadius;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private GameObject _model;
    [SerializeField] private ParticleSystem _fireEffect;
    [SerializeField] private AudioClip _explosionAudioClip;

    private ParticleSystem _fire;
    private ParticleSystem _explosion;
    private AudioSource _audioSource;

    private void OnEnable()
    {
        StartCoroutine(SetFire());
        _audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator SetFire()
    {
        _animator.Play("Fire");

        _fire = Instantiate(_fireEffect,this.transform);
        _fire.transform.position = transform.position + new Vector3(0, 0.2f, 0);

        float elapsedTime = 0;

        while (elapsedTime < _lifeTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Explosion();
    }

    private IEnumerator DestroyAfterShowExplosion()
    { 
        float elapsedTime = 0;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void Explosion()
    {
        _model.gameObject.SetActive(false);
        _explosion = Instantiate(_explosionEffect, this.transform);
        _explosion.transform.position = transform.position;
        _fire.gameObject.SetActive(false);
        _model.gameObject.SetActive(false);

        _audioSource.clip = _explosionAudioClip;
        _audioSource.Play();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _damageRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out Player player))
            {
                player.TakeDamage(_damage);
            }
            if (hitCollider.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(_damage);
            }
            if(hitCollider.TryGetComponent(out DestructibleObject destructibleObject))
            {
                destructibleObject.Hit();
            }
        }

        StartCoroutine(DestroyAfterShowExplosion());           
    }
}