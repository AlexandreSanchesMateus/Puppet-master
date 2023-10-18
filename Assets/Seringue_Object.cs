using Game;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Seringue_Object : Trap, IPickable
{
    public UnityEvent OnActivate;

    [SerializeField, Required] private Rigidbody m_rigidbody;

	private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            InflictFullDamageToPlayer();
            OnActivate?.Invoke();
        }
    }

	public void Take ( Transform parent )
	{
		transform.SetParent(parent);

		transform.transform.position = parent.transform.position;
		transform.localRotation = Quaternion.identity;
		m_rigidbody.useGravity = false;
		m_rigidbody.isKinematic = true;
	}

	public void Release ()
    {
	    transform.SetParent(null);

	    m_rigidbody.useGravity = true;
	    m_rigidbody.isKinematic = false;
    }
}
