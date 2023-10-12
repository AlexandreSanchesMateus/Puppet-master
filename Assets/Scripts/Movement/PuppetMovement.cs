using NaughtyAttributes;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Cinemachine;

public class PuppetMovement : MonoBehaviour
{
    [SerializeField, BoxGroup("Dependencies")] private PuppetPhysic _puppetPhysic;

    [SerializeField, BoxGroup("Settings")] private CinemachineVirtualCamera _camera;
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

    /*private struct LegData
    {
        public ELegState state;

        public bool initialize;

        public float timer;
        public Coroutine coroutine;

        public void Deinitialize()
        {
            state = ELegState.STOP;
            initialize = false;
        }

        public enum ELegState
        {
            STOP,
            FORWARD,
            BACKWARD,
            TURN,
        }
    }*/

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _camera.gameObject.transform.localEulerAngles = _camera.gameObject.transform.localEulerAngles - new Vector3(0, 5, 0);
            // _camera.transform.localRotation = Quaternion.Euler(_camera.transform.localRotation.x, _camera.transform.localRotation.y + 10, _camera.transform.localRotation.z);
        }
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
                CheckMovementInput();
                //_camera.TurnLeft(_turnSpeed);
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
            //_camera.StopTurn();

            if (_RCoroutine != null)
                StopCoroutine(_RCoroutine);
        }
        
        CheckMovementInput();

        /*if (!_RLeg.initialize)
        {
            if(value == Vector2.up)
            {
                if (_isRightLegPriority)
                    _RLeg.isMovingForward = true;
                else
                    _RLeg.isMovingForward = false;

                _RLeg.initialize = true;

                _RLeg.coroutine = StartCoroutine(StartTimer(_RLeg, _detectionTurnTime, () => {
                    _RLeg.isMoving = false;
                    _RLeg.isTurning = true;
                    _state = E.TURN;
                }));
            }
            else if(value == Vector2.down)
            {
                if (_isRightLegPriority)
                    _RLeg.isMovingForward = false;
                else
                    _RLeg.isMovingForward = true;

                _RLeg.initialize = true;
            }
        }
        else
        {
            if (_RLeg.isTurning)
            {
                if (value != Vector2.up)
                {
                    _RLeg.Deinitialize();
                }
            }
            else
            {
                if (value == Vector2.zero)
                {
                    if (_RLeg.coroutine != null)
                        StopCoroutine(_RLeg.coroutine);
                    
                    StartCoroutine(StartTimer(_RLeg, _stopInputTime, () => { _RLeg.Deinitialize(); _state = E.STOP; }));
                }
                else
                {


                    if (_RLeg.isMovingForward)
                    {
                        if(value == Vector2.down)
                        {
                            _RLeg.isMoving = true;
                            _state = E.FORWARD;
                        }
                    }
                    else
                    {
                        if(value == Vector2.up)
                        {
                            _RLeg.isMoving = true;
                            _state = E.BACKWARD;
                        }
                    }
                }
            }
        }*/

        /*if (value == Vector2.up)
        {
            // start timer to turn
            _RLeg.coroutine = StartCoroutine(StartTimer(_RLeg, _detectionTurnTime, () => {
                _RLeg.isMoving = false;
                _RLeg.isTurning = true;
            }));

            if (_isRightLegPriority)
            {
                _RLeg.isMovingForward = true;
            }
        }
        else
        {
            if (_RLeg.coroutine != null)
                StopCoroutine(_RLeg.coroutine);

            if (value == Vector2.down)
            {
                // is Moving true
                _LLeg.isMoving = true;
            }
            else if (value == Vector2.zero)
            {
                // Start timer to stop character
                StartCoroutine(StartTimer(_RLeg, _stopInputTime, _RLeg.Deinitialize));
            }
        }*/
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
                //_camera.TurnRight(_turnSpeed);
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
                /*if (_legsState.Item1 == ELegState.TURN && _legsState.Item2 == ELegState.NEUTRAL)
                {
                    // turn to the right
                    _verticalMovement = EVerticalMovement.TURNNING;
                    _camera.TurnRight(_turnSpeed);

                    if (_inputCoroutine != null)
                        StopCoroutine(_inputCoroutine);

                    _inputCoroutine = StartCoroutine(StartTimer(_stopInputTime, () => _verticalMovement = EVerticalMovement.STOP));
                }
                else if (_legsState.Item2 == ELegState.TURN && _legsState.Item1 == ELegState.NEUTRAL)
                {
                    // turn to the left
                    _verticalMovement = EVerticalMovement.TURNNING;
                    _camera.TurnLeft(_turnSpeed);

                    if (_inputCoroutine != null)
                        StopCoroutine(_inputCoroutine);

                    _inputCoroutine = StartCoroutine(StartTimer(_stopInputTime, () => _verticalMovement = EVerticalMovement.STOP));
                }
                else */
                
                
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
                        _puppetPhysic.Direction = Vector2.zero;
                    }));

                    _nextLegsState = (_legsState.Item1 == ELegState.FORWARD ? ELegState.BACKWARD : ELegState.FORWARD, _legsState.Item2 == ELegState.FORWARD ? ELegState.BACKWARD : ELegState.FORWARD);

                    SetDirection();
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

    private void SetDirection()
    {
        // Debug.Log(_camera.rotation.z + "    " + _camera.localRotation.z);
        _camera.transform.rotation = Quaternion.Euler(0, _camera.transform.rotation.y + 20, 0);

        // Vector2 cameraDirection = new Vector2(Mathf.Cos(_camera.rotation.y), Mathf.Sin(_camera.rotation.y));
        _puppetPhysic.Direction = (_verticalMovement == EVerticalMovement.FORWARD ? Vector2.up : Vector2.down) * _movementSpeed;
        // Debug.DrawRay(transform.position, new Vector3( cameraDirection.x, 0 , cameraDirection.y), Color.green, 100);
    }
}
