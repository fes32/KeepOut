using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _maxZRotateAngle;
    [SerializeField] private float _minZRotateAngle;
    [SerializeField] private GameObject _head;

    private PlayerInput _menu;
    private Vector2 _rotate ;
    private Vector3 _rotation;

    private void Start()
    {
        _menu = new PlayerInput();
        _menu.Enable();
    }

    private void Update()
    {
        _rotate = _menu.Menu.Look.ReadValue<Vector2>();

        float scaleRotateSpeed = _rotateSpeed * Time.deltaTime ;

        float targetZRotation = _rotation.z + _rotate.x * scaleRotateSpeed;

        if (targetZRotation > _minZRotateAngle & targetZRotation<_maxZRotateAngle)
            _rotation.z = targetZRotation;

        _rotation.x = Mathf.Clamp(_rotation.x-_rotate.y * scaleRotateSpeed,-40,30);

        _head.transform.localEulerAngles =_rotation;
    }




}
