using NaughtyAttributes;
using UnityEngine;

public class CutRope : MonoBehaviour, ICutable
{
	[SerializeField, Required] Ropes m_ropes;

	[SerializeField] private Rigidbody m_rigidbody;
	public Rigidbody Rigidbody => m_rigidbody;

	int index = 0;

	private void Start ()
	{
		m_rigidbody.velocity = Vector3.zero;
	}

	[Button]
	public void SetRigidbody ()
	{
		m_rigidbody = this.GetComponent<Rigidbody>();
	}

	public void Cut ()
	{
		if (m_ropes)
		{
			if (!m_ropes.HasBeenCut)
			{
				m_ropes.DestroyAfterTime();
			}

			//Destroy(m_ropes.TopLink?.gameObject.GetComponent<CharacterJoint>());
			//m_ropes.TopLink?.Rigidbody?.AddForce(Vector3.down * 1000);
		}

		Destroy(this.gameObject);
	}
}
