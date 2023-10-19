using UnityEngine;

public class SharpWeapon : Weapons
{
    protected override void OnCollisionEnter ( Collision collision )
    {
		base.OnCollisionEnter ( collision );

	    if (collision.gameObject.TryGetComponent(out ICutable toCut))
		{
			Cut(toCut);
		}
	}

	protected void OnTriggerEnter ( Collider other )
    {
	    if (other.gameObject.TryGetComponent(out ICutable toCut))
	    {
		    Cut(toCut);
	    }
    }

	protected void Cut ( ICutable toCut )
	{
		toCut.Cut();
		OnCut?.Invoke();
	}
}
