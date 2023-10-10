using NaughtyAttributes;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuppetPhysic : MonoBehaviour
{
    [SerializeField, BoxGroup("Dependences")] private BalanceForce _balanceManager;

    [SerializeField, BoxGroup("Down Force")] private Rigidbody _downRb;
    [SerializeField, BoxGroup("Down Force")] private Transform _applyPoint;
    [SerializeField, BoxGroup("Down Force")] private float _downForce;

    [SerializeField, BoxGroup("Suspension Settings")] private float _restLength;
    [SerializeField, BoxGroup("Suspension Settings")] private float _springStiffness;
    [SerializeField, BoxGroup("Suspension Settings")] private float _damperStiffness;
    [SerializeField, BoxGroup("Suspension Settings")] private float _legLength;
    [SerializeField, BoxGroup("Suspension Settings")] private float _minLength;

    private float _lastLenght;
    private float _springLength;

    private bool _isGrounded;

    private EPuppetPhysic _state;

    private enum EPuppetPhysic
    {
        STANDING,
        NOT_GROUNDED,
        DISABLE,
    }

    private void OnDrawGizmosSelected()
    {
        // Max extention leg
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, -transform.up);

        // Rest lenght
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(-transform.up, 0.04f);
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case EPuppetPhysic.STANDING:
                if (Physics.Raycast(gameObject.transform.position, -transform.up, out RaycastHit info, _legLength))
                {
                    _lastLenght = _springLength;
                    _springLength = Mathf.Clamp(info.distance, _minLength, _legLength);

                    float springVelocity = (_lastLenght - _springLength) / Time.fixedDeltaTime;
                    float springForce = _springStiffness * (_springLength - _restLength);
                    float damperForce = _damperStiffness * springVelocity;

                    Vector3 suspensionForce = (springForce + damperForce) * -transform.up;
                    _downRb.AddForceAtPosition(suspensionForce, info.point);
                    _isGrounded = true;

                    Debug.DrawLine(gameObject.transform.position, info.point, Color.green);
                }
                else
                {
                    _state = EPuppetPhysic.NOT_GROUNDED;
                    _balanceManager.EnableBalance(false);
                }
                break;

            case EPuppetPhysic.NOT_GROUNDED:
                break;

            case EPuppetPhysic.DISABLE:
                break;
        }
    }
}
