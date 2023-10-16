using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private float _checkRadius;
    [SerializeField] private LayerMask _layer;

    private IPickable _currentObject;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0.8f, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, _checkRadius);
    }

    public void TakeObject()
    {
        if (_currentObject != null) return;

        if(Physics.SphereCast(transform.position, _checkRadius, Vector3.down, out RaycastHit info, _layer))
        {
            if(info.collider.gameObject.TryGetComponent<IPickable>(out _currentObject))
            {
                _currentObject.Take(transform);
            }
        }
    }

    public void ReleaseObject()
    {
        if (_currentObject == null) return;

        _currentObject.Release();
        _currentObject = null;
    }
}
