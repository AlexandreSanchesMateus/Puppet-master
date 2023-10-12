using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PuppetPhysic : MonoBehaviour
{
    [SerializeField, BoxGroup("Dependences")] private BalanceForce _balanceManager;

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
    public Vector3 Direction { get; set; } = Vector3.zero;

    private float _timer;

    private EPuppetPhysic _state = EPuppetPhysic.NOT_GROUNDED;

    private enum EPuppetPhysic
    {
        STANDING,
        NOT_GROUNDED,
        RECOVER,
        DISABLE,
    }

    private void OnDrawGizmosSelected()
    {
        // Max extention leg
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
                    Vector3 torque = Vector3.Project(Vector3.Cross(Camera.main.transform.forward, -_downRb.transform.up), Vector3.up);
                    _downRb.AddTorque(torque * _turnForce * Time.fixedDeltaTime);
                    Debug.DrawRay(_applyPoint.position, Vector3.down * _legLength, Color.green);
                }
                else
                {
                    _state = EPuppetPhysic.NOT_GROUNDED;
                    _balanceManager.EnableBalance(false);
                }

                CheckBodyTilting();
                break;

            case EPuppetPhysic.NOT_GROUNDED:
                if (Physics.Raycast(_applyPoint.position, _applyPoint.right, _legLength, _mask))
                {
                    _state = EPuppetPhysic.STANDING;
                    _downRb.AddForceAtPosition(Vector3.down * _downForce, _applyPoint.position);
                    _balanceManager.EnableBalance(true);
                }

                Debug.DrawRay(_applyPoint.position, Vector3.down * _legLength, Color.red);

                CheckBodyTilting();
                break;

            case EPuppetPhysic.RECOVER:
                if (Vector3.Dot(Vector3.up, -_applyPoint.right) > 0.5f)
                {
                    _state = EPuppetPhysic.STANDING;
                    _onPuppetFullyRecover?.Invoke();
                }
                break;

            case EPuppetPhysic.DISABLE:
                if(_downRb.velocity.magnitude <= 0.1f)
                {
                    _timer += Time.fixedDeltaTime;

                    if (_timer >= _disableTime)
                    {
                        _state = EPuppetPhysic.RECOVER;
                        _balanceManager.EnableBalance(true);
                    }
                }
                break;
        }
    }

    private void CheckBodyTilting()
    {
        if(Vector3.Dot(Vector3.up, -_applyPoint.right) <= 0.2f)
        {
            _state = EPuppetPhysic.DISABLE;
            _balanceManager.EnableBalance(false);
            _onPuppetDisable?.Invoke();
            _timer = 0;
        }
    }
}
