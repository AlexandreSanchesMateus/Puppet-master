using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PuppetPhysic : MonoBehaviour
{
    [SerializeField, BoxGroup("Dependences")] private BalanceForce _balanceManager;
    [SerializeField, BoxGroup("Dependences")] private BalanceForce _headManager;
    [SerializeField, BoxGroup("Dependences")] private PuppetInteraction _interactionManager;

    [SerializeField, BoxGroup("Setup")] private Rigidbody _downRb;
    [SerializeField, BoxGroup("Setup")] private Transform _applyPoint;

    [SerializeField, BoxGroup("Settings")] private LayerMask _mask;
    [SerializeField, BoxGroup("Settings")] private float _legLength;
    [SerializeField, BoxGroup("Settings")] private float _disableTime;
    [SerializeField, BoxGroup("Settings")] private float _downForce;
    [SerializeField, BoxGroup("Settings")] private float _turnForce;

    [SerializeField, Foldout("Event")] private UnityEvent _onPuppetDisable;
    [SerializeField, Foldout("Event")] private UnityEvent _onPuppetFullyRecover;

    public Vector3 Movement { get; set; } = Vector2.zero;
    public Vector3 GetHipsPosition => _downRb.transform.position;

    private float _timer;
    private EPuppetPhysic _state = EPuppetPhysic.NOT_GROUNDED;

    public enum EPuppetPhysic
    {
        STANDING,
        NOT_GROUNDED,
        RECOVER,
        DISABLE,
    }

    private void OnDrawGizmosSelected()
    {
        if (_applyPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_applyPoint.position, _applyPoint.position + (Vector3.down * _legLength));
    }

    private void FixedUpdate()
    {
        // Mouvement
        switch (_state)
        {
            case EPuppetPhysic.STANDING:
                if (Physics.Raycast(_applyPoint.position, Vector3.down, _legLength, _mask))
                {
                    _downRb.AddForceAtPosition(Vector3.down * _downForce + Movement, _applyPoint.position);

                    // Rotation
                    Vector3 torque = Vector3.Project(Vector3.Cross(Camera.main.transform.forward, -_downRb.transform.forward), Vector3.up);
                    _downRb.AddTorque(torque * _turnForce * Time.fixedDeltaTime);
                    Debug.DrawRay(_applyPoint.position, Vector3.down * _legLength, Color.green);
                }
                else
                {
                    _state = EPuppetPhysic.NOT_GROUNDED;
                    _balanceManager.EnableBalance(false);
                    _headManager.EnableBalance(false);
                    _interactionManager.EnableleInteraction(false);
                }

                CheckBodyTilting();
                break;

            case EPuppetPhysic.NOT_GROUNDED:
                if (Physics.Raycast(_applyPoint.position, Vector3.down, _legLength, _mask))
                {
                    _state = EPuppetPhysic.STANDING;
                    _balanceManager.EnableBalance(true);
                    _headManager.EnableBalance(true);
                    _interactionManager.EnableleInteraction(true);
                }

                Debug.DrawRay(_applyPoint.position, Vector3.down * _legLength, Color.red);
                CheckBodyTilting();
                break;

            case EPuppetPhysic.RECOVER:
                if (Vector3.Dot(Vector3.up, _applyPoint.up) > 0.5f)
                {
                    _state = EPuppetPhysic.STANDING;
                    _interactionManager.EnableleInteraction(true);
                    _onPuppetFullyRecover?.Invoke();
                }
                _downRb.AddForceAtPosition(Vector3.down * _downForce, _applyPoint.position);
                break;

            case EPuppetPhysic.DISABLE:
                if(_downRb.velocity.magnitude <= 0.1f)
                {
                    _timer += Time.fixedDeltaTime;

                    if (_timer >= _disableTime)
                    {
                        _state = EPuppetPhysic.RECOVER;
                        _balanceManager.EnableBalance(true);
                        _headManager.EnableBalance(true);
                    }
                }
                break;
        }
    }

    private void CheckBodyTilting()
    {
        if(Vector3.Dot(Vector3.up, _applyPoint.up) <= 0.2f)
        {
            DisablePhysic();
        }
    }

    public void SetPuppetPhysicToDisable() => DisablePhysic();
    
    private void DisablePhysic()
    {
        _state = EPuppetPhysic.DISABLE;
        _balanceManager.EnableBalance(false);
        _headManager.EnableBalance(false);
        _interactionManager.EnableleInteraction(false);
        _onPuppetDisable?.Invoke();
        _timer = 0;
    }
}
