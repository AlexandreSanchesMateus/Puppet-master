using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Game;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Weapons : MonoBehaviour, IPickable
{
    public UnityEvent OnCut;

    [SerializeField, Required] protected Rigidbody m_rigidbody;

    [SerializeField, BoxGroup("Settings")] protected int m_damage = 10;

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
    protected virtual void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.TryGetComponent(out IDamageable health))
		{
			health.TakeDamage(m_damage);
		}

		OnCut?.Invoke();

		StartCoroutine(CheckIfStillGrounded());
    }

    private IEnumerator CheckIfStillGrounded()
    {
	    yield return new WaitForSeconds(2f);

	    if (this.transform.position.y <= -2)
	    {
		    this.transform.position = new Vector3(this.transform.position.x, 1, this.transform.position.z);
		    m_rigidbody.velocity = Vector3.zero;
		}
    }


	[Button]
    public void DebugCut()
    {
        OnCut?.Invoke();
    }    
}
