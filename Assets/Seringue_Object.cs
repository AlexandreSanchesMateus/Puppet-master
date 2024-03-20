using Game;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Seringue_Object : Trap, IPickable
{
    public UnityEvent OnActivate;

    [SerializeField, Required] private Rigidbody m_rigidbody;


    private Vector3 m_starterPoint;

    protected override void Start()
    {
        base.Start();
        m_starterPoint = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            InflictFullDamageToPlayer();
            OnActivate?.Invoke();
        }
    }

	public void ResetToOriginPosition()
	{
        transform.position = m_starterPoint;
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
