using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{

    private Transform _hinge;
    private Transform _player;

    public bool isOpen = false;
    [SerializeField]
    [Tooltip("How fast the door opens")]
    private float _speed = 1f;
    [SerializeField]
    private float _rotationAmount = 90f;
    [SerializeField]
    private float _forwardDirection = 0;

    private Vector3 _startRotation;
    private Vector3 _forward;

    private Coroutine _animationCoroutine;

    private void Awake()
    {
        _hinge = GameObject.Find("Hinge").transform;
        _player = GameObject.Find("Player").transform;

        _startRotation = _hinge.rotation.eulerAngles;
        _forward = _hinge.right;
    }

    public void Open(Vector3 UserPosition)
    {
        if (!isOpen)
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }

            float dot = Vector3.Dot(_forward, (UserPosition - _hinge.position).normalized);
            _animationCoroutine = StartCoroutine(DoRotationOpen(dot));
        }
    }

    private IEnumerator DoRotationOpen(float forwardAmount)
    {
        Quaternion startRotation = _hinge.rotation;
        Quaternion endRotation;

        if (forwardAmount >= _forwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRotation.y - _rotationAmount, 0));
        } else
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRotation.y + _rotationAmount, 0));
        }

        isOpen = true;

        float time = 0;
        while (time < 1)
        {
            _hinge.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * _speed;
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            if (_animationCoroutine != null)
            {
                _animationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = _hinge.rotation;
        Quaternion endRotation = Quaternion.Euler(_startRotation);

        isOpen = false;

        float time = 0;
        while (time < 1)
        {
            _hinge.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * _speed;
        }
    }

    public override void Interact()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open(_player.position);
        }
    }
}
