using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PuppetInteraction : MonoBehaviour
{
    [SerializeField] private float _snapForce;

    [SerializeField, BoxGroup] private HandData _LHandData;
    [SerializeField, BoxGroup] private HandData _RHandData;

    [SerializeField, Foldout("Events")] private UnityEvent _onRightHandChange;
    [SerializeField, Foldout("Events")] private UnityEvent _onLeftHandChange;

    private bool _enable = true;

    [System.Serializable]
    private class HandData
    {
        public Rigidbody rb;
        public Transform clavicle;
        public Hand interaction;
        public Vector3 handPos => interaction.gameObject.transform.position;

        private Vector2 lastHandPosition = Vector2.zero;

        [field: SerializeField] public Vector3 IdleOffset { get; private set; }
        [field: SerializeField] public Vector3 ForwardOffset { get; private set; }
        [field: SerializeField] public Vector3 BackwardOffset { get; private set; }
        [field: SerializeField] public Vector3 UpwardOffset { get; private set; }
        [field: SerializeField] public Vector3 DownwardOffset { get; private set; }

        public EHandPosition HandState { get; private set; } = EHandPosition.IDLE;

        public enum EHandPosition
        {
            IDLE,
            FORWARD,
            BACKWARD,
            UPWARD,
            DOWNWARD
        }

        public void SetHandPosition(Vector2 pos)
        {
            Vector2 inputChange = new Vector2(Mathf.Abs(pos.x - lastHandPosition.x), Mathf.Abs(pos.y - lastHandPosition.y));
            lastHandPosition = pos;

            if (inputChange.x > 0.5f)
            {
                if (pos.x < 0)
                    HandState = EHandPosition.FORWARD;
                else if (pos.x > 0)
                    HandState = EHandPosition.BACKWARD;
                else
                {
                    if (pos.y < 0)
                        HandState = EHandPosition.DOWNWARD;
                    else if (pos.y > 0)
                        HandState = EHandPosition.UPWARD;
                    else
                        HandState = EHandPosition.IDLE;
                }
            }
            else if (inputChange.y > 0.5f)
            {
                if (pos.y < 0)
                    HandState = EHandPosition.DOWNWARD;
                else if (pos.y > 0)
                    HandState = EHandPosition.UPWARD;
                else
                {
                    if (pos.x < 0)
                        HandState = EHandPosition.FORWARD;
                    else if (pos.x > 0)
                        HandState = EHandPosition.BACKWARD;
                    else
                        HandState = EHandPosition.IDLE;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(_RHandData.clavicle != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(_RHandData.clavicle.position + (_RHandData.clavicle.rotation * _RHandData.IdleOffset), 0.05f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_RHandData.clavicle.position + (_RHandData.clavicle.rotation * _RHandData.ForwardOffset), 0.05f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_RHandData.clavicle.position + (_RHandData.clavicle.rotation * _RHandData.BackwardOffset), 0.05f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_RHandData.clavicle.position + (_RHandData.clavicle.rotation * _RHandData.UpwardOffset), 0.05f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_RHandData.clavicle.position + (_RHandData.clavicle.rotation * _RHandData.DownwardOffset), 0.05f);
        }

        if(_LHandData.clavicle != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(_LHandData.clavicle.position + (_LHandData.clavicle.rotation * _LHandData.IdleOffset), 0.05f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere( _LHandData.clavicle.position + (_LHandData.clavicle.rotation * _LHandData.ForwardOffset), 0.05f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_LHandData.clavicle.position + (_LHandData.clavicle.rotation * _LHandData.BackwardOffset), 0.05f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_LHandData.clavicle.position + (_LHandData.clavicle.rotation * _LHandData.UpwardOffset), 0.05f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere( _LHandData.clavicle.position + (_LHandData.clavicle.rotation * _LHandData.DownwardOffset), 0.05f);
        }
    }

    private void FixedUpdate()
    {
        if (!_enable) return;

        UpdateHandPosition(_LHandData);
        UpdateHandPosition(_RHandData);
    }

    public void SetRightArmAction(Vector2 value)
    {
        _RHandData.SetHandPosition(value);
        _onRightHandChange?.Invoke();
    }

    public void SetLeftArmAction(Vector2 value)
    {
        _LHandData.SetHandPosition(value);
        _onLeftHandChange?.Invoke();
    }

    public void SetLeftHandInteraction(bool interact)
    {
        if (interact)
        {
            _LHandData.interaction.TakeObject();
        }
        else
        {
            _LHandData.interaction.ReleaseObject();
        }
    }

    public void SetRightHandInteraction(bool interact)
    {
        if (interact)
        {
            _RHandData.interaction.TakeObject();
        }
        else
        {
            _RHandData.interaction.ReleaseObject();
        }
    }

    private void UpdateHandPosition(HandData info)
    {
        Vector3 target = info.clavicle.position + (info.clavicle.rotation * info.IdleOffset);
        switch (info.HandState)
        {
            case HandData.EHandPosition.FORWARD:
                target = info.clavicle.position + (info.clavicle.rotation * info.ForwardOffset);
                break;

            case HandData.EHandPosition.BACKWARD:
                target = info.clavicle.position + (info.clavicle.rotation * info.BackwardOffset);
                break;

            case HandData.EHandPosition.UPWARD:
                target = info.clavicle.position + (info.clavicle.rotation * info.UpwardOffset);
                break;

            case HandData.EHandPosition.DOWNWARD:
                target = info.clavicle.position + (info.clavicle.rotation * info.DownwardOffset);
                break;
        }

        Debug.DrawRay(info.handPos, target - info.handPos, Color.cyan);
        info.rb.AddForceAtPosition((target - info.handPos) * _snapForce, info.handPos);
    }

    public void EnableleInteraction(bool enable)
    {
        _enable = enable;
        if(_enable)
        {
            _LHandData.rb.useGravity = false;
            _RHandData.rb.useGravity = false;
        }
        else
        {
            _LHandData.rb.useGravity = true;
            _RHandData.rb.useGravity = true;
        }
    }
}
