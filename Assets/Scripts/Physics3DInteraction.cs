using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Physics3DInteraction : MonoBehaviour
{
	[SerializeField] UnityEvent<Collider> _onTriggerEnter;
	[SerializeField] UnityEvent<Collider> _onTriggerExit;
	[SerializeField] UnityEvent<Collider> _onTriggerStay;

	[SerializeField] UnityEvent<Collision> _onCollisionEnter;

	public event UnityAction<Collider> TriggerEnter3D { add => _onTriggerEnter.AddListener(value); remove => _onTriggerEnter.RemoveListener(value); }
	public event UnityAction<Collider> TriggerExit3D { add => _onTriggerExit.AddListener(value); remove => _onTriggerExit.RemoveListener(value); }
	public event UnityAction<Collider> TriggerStay3D { add => _onTriggerStay.AddListener(value); remove => _onTriggerStay.RemoveListener(value); }

	public event UnityAction<Collision> CollisionEnter3D { add => _onCollisionEnter.AddListener(value); remove => _onCollisionEnter.RemoveListener(value); }

	private void OnCollisionEnter ( Collision collision )
	{
		_onCollisionEnter.Invoke(collision);
	}

	private void OnTriggerEnter ( Collider other )
	{
		_onTriggerEnter.Invoke(other);
	}

	private void OnTriggerExit ( Collider other )
	{
		_onTriggerExit.Invoke(other);
	}

	private void OnTriggerStay ( Collider other )
	{
		_onTriggerStay.Invoke(other);
	}
}
