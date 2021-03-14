using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private DropItem[] _dropItems;
    [SerializeField] private AudioClip _destroyAudioClip;
    [SerializeField] private GameObject _model;
    [SerializeField] private GameObject _destructionEffect;

    private Decor _decor;
    private AudioSource _audioSource;
    private float _durationAudioClip;
    private float _minPitch = 0.5f;
    private float _maxPitch = 0.5f;
    private bool _isActive = true;
    

    private void OnEnable()
    {
        _decor = GetComponent<Decor>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _destroyAudioClip;
        _durationAudioClip = _destroyAudioClip.length;
    }

    private void Destroy()
    {
        _model.SetActive(false);
        Instantiate(_destructionEffect, transform);
        StartCoroutine(DestroyAfterPlaingAudio());
    }

    private IEnumerator DestroyAfterPlaingAudio()
    {
        _audioSource.pitch= Random.Range(_minPitch, _maxPitch);
        _audioSource.Play();

        float elapsedTime = 0;

        while(elapsedTime<_durationAudioClip)
        {
            elapsedTime += Time.deltaTime;
          yield return null;
        }

       Destroy(gameObject);
    }

    public void Hit()
    {
        if (_isActive)
        {
            _isActive = false;
            _decor = GetComponent<Decor>();

            foreach (var item in _dropItems)
            {
                if (item.DropChance >= Random.Range(0, 100))
                {
                    float y = item.transform.position.y;
                    var obj = Instantiate(item, _decor.Cell);
                    obj.transform.position = transform.position + new Vector3(0, y, 0);

                    break;
                }
            }
            Destroy();
        }
    }
}