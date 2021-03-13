using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Player _player;

    private PlayerInputAction _input;

    public event UnityAction Pause;

    private void OnEnable()
    {
        _input = new PlayerInputAction();
        _input.Enable();

        _input.Player.Attack.performed += ctx => OnAttack();
        _input.Player.Healing.performed += ctx => OnHealing();
        _input.Player.Pause.performed += ctx => OnPause();
        _input.Player.SetSingTable.performed += ctx => OnSetSingTable();
    }

    private void Update()
    {
        Vector3 _direction = _input.Player.Move.ReadValue<Vector2>();
        _player.Move(_direction);

        Vector3 _rotate = _input.Player.Look.ReadValue<Vector2>();
        _player.Look(_rotate);
    }

    private void OnSetSingTable()
    {
        _player.SetSingTable();
    }

    private void OnAttack()
    {
        _player.Attack();
    }

    private void OnHealing()
    {
        _player.Healing();
    }

    private void OnPause()
    {
        Pause?.Invoke();
    }
}