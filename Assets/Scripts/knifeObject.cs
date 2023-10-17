using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class knifeObject : MonoBehaviour,IPickable
{
    public UnityEvent OnCut;

    [SerializeField, Required] private Rigidbody m_rigidbody;

	public void Take(Transform parent)
    {
        transform.SetParent(parent);

        transform.localPosition = parent.localPosition;

        m_rigidbody.useGravity = false;
	}

    public void Release()
    {
        transform.SetParent(null);

        m_rigidbody.useGravity = true;
	}
    private void OnCollisionEnter(Collision collision)
    {
	    if (collision.gameObject.TryGetComponent<ICutable>(out ICutable toCut))
	    {
		    toCut.Cut();
	    }
	    OnCut?.Invoke();
	}

	private void OnTriggerEnter ( Collider other )
	{
		if (other.gameObject.TryGetComponent<ICutable>(out ICutable toCut))
		{
			toCut.Cut();
		}
		OnCut?.Invoke();
	}

	[Button]
    public void DebugCut()
    {
        OnCut?.Invoke();
    }    
}
