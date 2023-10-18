using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private SphereCollider m_sphereCollider;

    [SerializeField] private Transform m_weaponHolder;

    private IPickable _currentObject;

    public void Start()
    {
	    if (m_sphereCollider.enabled) m_sphereCollider.enabled = false;
	}

    public void TakeObject()
    {
        if (_currentObject != null) return;

        m_sphereCollider.enabled = true;
    }

    public void OnTriggerEnter(Collider _collider)
    {
	    if (_collider.gameObject.TryGetComponent(out IPickable pickUp))
	    {
		    pickUp.Take(transform);
		    m_sphereCollider.enabled = false;
		}
	}

    public void ReleaseObject()
    {
        m_sphereCollider.enabled = false;

        if (_currentObject == null) return;

		_currentObject.Release();
        _currentObject = null;
    }
}
