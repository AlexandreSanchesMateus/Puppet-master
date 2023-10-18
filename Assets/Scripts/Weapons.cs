using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Game;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Weapons : MonoBehaviour,IPickable
{
    public UnityEvent OnCut;

    [SerializeField, Required] private Rigidbody m_rigidbody;

    [SerializeField, BoxGroup("Settings")] private int m_damage = 10;

	public void Take(Transform parent)
	{
		transform.SetParent(parent);

        transform.transform.position = parent.transform.position;
        transform.localRotation = Quaternion.identity;
        m_rigidbody.useGravity = false;
        m_rigidbody.isKinematic = true;
    }

    public void Release()
    {
        transform.SetParent(null);

        m_rigidbody.useGravity = true;
        m_rigidbody.isKinematic = false;
	}
	private void OnCollisionEnter(Collision collision)
    {
	    if (collision.gameObject.TryGetComponent(out ICutable toCut))
	    {
		    toCut.Cut();

		    OnCut?.Invoke();
		}

		if (collision.gameObject.TryGetComponent(out IDamageable health))
		{
			health.TakeDamage(m_damage);

			OnCut?.Invoke();
		}
	}

	private void OnTriggerEnter ( Collider other )
	{
		if (other.gameObject.TryGetComponent<ICutable>(out ICutable toCut))
		{
			toCut.Cut();

			OnCut?.Invoke();
		}
	}

	[Button]
    public void DebugCut()
    {
        OnCut?.Invoke();
    }    
}
