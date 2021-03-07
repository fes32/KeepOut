using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Player _player;

    private PlayerInput _input;
    private Vector2 _direction;
    private Vector2 _rotate;
    private Vector2 _rotation;

    public event UnityAction Pause;

    private void OnEnable()
    {
        _input = new PlayerInput();
        _input.Enable();

        _input.Player.Attack.performed += ctx => OnAttack();
        _input.Player.Healing.performed += ctx => OnHealing();
        _input.Player.Pause.performed += ctx => OnPause();
        _input.Player.SetSingTable.performed += ctx => OnSetSingTable();
    }

    private void Update()
    {
        _direction = _input.Player.Move.ReadValue<Vector2>();
        _player.Move(_direction);

        _rotate = _input.Player.Look.ReadValue<Vector2>();
        _player.Look(_rotate);
    }

    private void OnSetSingTable()
    {
        _player.SetSingTable();
    }

    private void OnAttack()
    {
        _player.TryAttack();
    }

    private void OnHealing()
    {
        _player.TryHealing();
    }

    private void OnPause()
    {
        Pause?.Invoke();
    }
}