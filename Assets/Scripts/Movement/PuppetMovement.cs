using NaughtyAttributes;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PuppetMovement : MonoBehaviour
{
    [SerializeField, BoxGroup("Dependencies")] private PuppetPhysic _puppetPhysic;

    [SerializeField, BoxGroup("Settings")] private Transform _camera;
    [SerializeField, BoxGroup("Settings")] private float _movementSpeed;
    [SerializeField, BoxGroup("Settings")] private float _turnSpeed;

    [SerializeField] private float _detectionTurnTime;
    [SerializeField] private float _stopInputTime;

    [SerializeField] private (ELegState, ELegState) _legsState;
    [SerializeField] private (ELegState, ELegState) _nextLegsState;

    private EVerticalMovement _verticalMovement;

    private Coroutine _inputCoroutine = null;

    private Coroutine _RCoroutine = null;
    private Coroutine _LCoroutine = null;

    private Coroutine _TurnCoroutine = null;

    
    public enum EVerticalMovement
    {
        STOP,
        FORWARD,
        BACKWARD
    }

    public enum ELegState
    {
        NEUTRAL,
        FORWARD,
        BACKWARD,
        TURN,
    }

    public void SetRightLegValue(Vector2 value)
    {
        if(value == Vector2.up)
        {
            _legsState.Item2 = ELegState.FORWARD;

            _RCoroutine = StartCoroutine(StartTimer(_detectionTurnTime, () =>
            {
                _verticalMovement = EVerticalMovement.STOP;
                _legsState.Item2 = ELegState.TURN;
                _TurnCoroutine = StartCoroutine(TurnCamera(true));
                CheckMovementInput();
            }));
        }
        else if(value == Vector2.down)
        {
            _legsState.Item2 = ELegState.BACKWARD;

            if (_RCoroutine != null)
                StopCoroutine(_RCoroutine);
        }
        else if(value == Vector2.zero)
        {
            _legsState.Item2 = ELegState.NEUTRAL;

            if (_TurnCoroutine != null)
                StopCoroutine(_TurnCoroutine);

            if (_RCoroutine != null)
                StopCoroutine(_RCoroutine);
        }
        
        CheckMovementInput();
    }

    public void SetLeftLegValue(Vector2 value)
    {
        if (value == Vector2.up)
        {
            _legsState.Item1 = ELegState.FORWARD;

            _LCoroutine = StartCoroutine(StartTimer(_detectionTurnTime, () =>
            {
                _verticalMovement = EVerticalMovement.STOP;
                _legsState.Item1 = ELegState.TURN;
                _TurnCoroutine = StartCoroutine(TurnCamera(false));
                CheckMovementInput();
            }));
        }
        else if (value == Vector2.down)
        {
            _legsState.Item1 = ELegState.BACKWARD;

            if (_LCoroutine != null)
                StopCoroutine(_LCoroutine);
        }
        else if (value == Vector2.zero)
        {
            _legsState.Item1 = ELegState.NEUTRAL;
            //_camera.StopTurn();

            if (_TurnCoroutine != null)
                StopCoroutine(_TurnCoroutine);

            if (_LCoroutine != null)
                StopCoroutine(_LCoroutine);
        }

        CheckMovementInput();
    }

    private void CheckMovementInput()
    {
        switch (_verticalMovement)
        {
            case EVerticalMovement.STOP:
                if (_legsState.Item1 == ELegState.FORWARD && _legsState.Item2 == ELegState.BACKWARD)
                {
                    // Mover backward
                    _verticalMovement = EVerticalMovement.BACKWARD;
                    _nextLegsState = (ELegState.BACKWARD, ELegState.FORWARD);

                    if (_inputCoroutine != null)
                        StopCoroutine(_inputCoroutine);

                    _inputCoroutine = StartCoroutine(StartTimer(_stopInputTime, () => _verticalMovement = EVerticalMovement.STOP));
                }
                else if (_legsState.Item1 == ELegState.BACKWARD && _legsState.Item2 == ELegState.FORWARD)
                {
                    // Move forward
                    _verticalMovement = EVerticalMovement.FORWARD;
                    _nextLegsState = (ELegState.FORWARD, ELegState.BACKWARD);

                    if (_inputCoroutine != null)
                        StopCoroutine(_inputCoroutine);

                    _inputCoroutine = StartCoroutine(StartTimer(_stopInputTime, () => _verticalMovement = EVerticalMovement.STOP));
                }
                break;

            case EVerticalMovement.BACKWARD:
            case EVerticalMovement.FORWARD:
                // Loop step action
                if (_legsState.Item1 == _nextLegsState.Item1 && _legsState.Item2 == _nextLegsState.Item2)
                {
                    if (_inputCoroutine != null)
                        StopCoroutine(_inputCoroutine);

                    _inputCoroutine = StartCoroutine(StartTimer(_stopInputTime, () =>
                    {
                        _verticalMovement = EVerticalMovement.STOP;
                        _puppetPhysic.Movement = Vector2.zero;
                    }));

                    _nextLegsState = (_legsState.Item1 == ELegState.FORWARD ? ELegState.BACKWARD : ELegState.FORWARD, _legsState.Item2 == ELegState.FORWARD ? ELegState.BACKWARD : ELegState.FORWARD);

                    _puppetPhysic.Movement = new Vector3(_camera.forward.x, 0, _camera.forward.z).normalized * _movementSpeed;
                }
                break;
        }
    }
    
    private IEnumerator StartTimer(float exitTime, Action onFinish)
    {
        float timer = 0;

        while (timer < exitTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        onFinish?.Invoke();
    }

    private IEnumerator TurnCamera(bool turnLeft)
    {
        while (true)
        {
            if(turnLeft)
                _camera.localEulerAngles = _camera.localEulerAngles - new Vector3(0, Time.deltaTime * _turnSpeed, 0);
            else
                _camera.localEulerAngles = _camera.localEulerAngles + new Vector3(0, Time.deltaTime * _turnSpeed, 0);

            yield return null;
        }
    }
}
