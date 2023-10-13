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

    [SerializeField, Foldout("Events")] private UnityEvent _onHandPositionChange;

    private bool _enable = true;

    [System.Serializable]
    private class HandData
    {
        public Rigidbody rb;
        public Transform handPos;
        public Transform clavicle;

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

        public void SetHandPosition(EHandPosition pos) => HandState = pos;
    }

    private void OnDrawGizmosSelected()
    {
        if(_RHandData.clavicle != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(_RHandData.clavicle.position + _RHandData.IdleOffset, 0.05f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_RHandData.clavicle.position + _RHandData.ForwardOffset, 0.05f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_RHandData.clavicle.position + _RHandData.BackwardOffset, 0.05f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_RHandData.clavicle.position + _RHandData.UpwardOffset, 0.05f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_RHandData.clavicle.position + _RHandData.DownwardOffset, 0.05f);
        }

        if(_LHandData.clavicle != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(_LHandData.clavicle.position + _LHandData.IdleOffset, 0.05f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_LHandData.clavicle.position + _LHandData.ForwardOffset, 0.05f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_LHandData.clavicle.position + _LHandData.BackwardOffset, 0.05f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_LHandData.clavicle.position + _LHandData.UpwardOffset, 0.05f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_LHandData.clavicle.position + _LHandData.DownwardOffset, 0.05f);
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
        _RHandData.SetHandPosition(ConvertInHandPos(value));
        _onHandPositionChange?.Invoke();
    }

    public void SetLeftArmAction(Vector2 value)
    {
        _LHandData.SetHandPosition(ConvertInHandPos(value));
        _onHandPositionChange?.Invoke();
    }

    private HandData.EHandPosition ConvertInHandPos(Vector2 direction)
    {
        if (direction == Vector2.up)
            return HandData.EHandPosition.UPWARD;
        else if (direction == Vector2.down)
            return HandData.EHandPosition.DOWNWARD;
        else if (direction == Vector2.left)
            return HandData.EHandPosition.FORWARD;
        else if (direction == Vector2.right)
            return HandData.EHandPosition.BACKWARD;

        return HandData.EHandPosition.IDLE;
    }

    private void UpdateHandPosition(HandData info)
    {
        Vector3 target = info.clavicle.position + info.IdleOffset;
        switch (info.HandState)
        {
            case HandData.EHandPosition.FORWARD:
                target = info.clavicle.position + info.ForwardOffset;
                break;

            case HandData.EHandPosition.BACKWARD:
                target = info.clavicle.position + info.BackwardOffset;
                break;

            case HandData.EHandPosition.UPWARD:
                target = info.clavicle.position + info.UpwardOffset;
                break;

            case HandData.EHandPosition.DOWNWARD:
                target = info.clavicle.position + info.DownwardOffset;
                break;
        }

        Debug.DrawRay(info.handPos.position, target - info.handPos.position, Color.cyan);
        info.rb.AddForceAtPosition(target - info.handPos.position * _snapForce, info.handPos.position);
    }

    public void DisablleInteraction(bool enable) => _enable = enable;
}
