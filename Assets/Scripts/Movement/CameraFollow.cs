using NaughtyAttributes;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteAlways]
public class CameraFollow : MonoBehaviour
{
    public float Angle => transform.rotation.y;

    [SerializeField, Required] private Transform _target;
    [SerializeField] private Vector3 _offset;

    private bool _isTurning;

    Coroutine _routine;

    private void Update()
    {
        if(_target != null)
            gameObject.transform.position = _target.position + _offset;
    }

    public void SetRotation(float angle) => transform.rotation = Quaternion.Euler(transform.rotation.x, angle, transform.rotation.y);
    public Vector2 GetCameraDirection() => new Vector2(Mathf.Cos(transform.rotation.y), Mathf.Sin(transform.rotation.y));


    public void TurnLeft(float rotationSpeed)
    {
        if(_routine == null)
            _routine = StartCoroutine(TurnCamera(rotationSpeed));
    }

    public void TurnRight(float rotationSpeed)
    {
        if(_routine == null)
            _routine = StartCoroutine(TurnCamera(-rotationSpeed));
    }

    public void StopTurn() => _isTurning = false;

    private IEnumerator TurnCamera(float speed)
    {
        Debug.Log("turn");
        _isTurning = true;
        while (_isTurning)
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + Time.deltaTime * speed, transform.localRotation.z);
            yield return null;
        }
        Debug.Log("stop");
    }
}
