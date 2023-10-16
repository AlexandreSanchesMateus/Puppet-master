using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class CombustionTrap : Trap, ICutable
{

	[SerializeField, BoxGroup("References")] private GameObject m_flameToActivate;
	[SerializeField, BoxGroup("References")] private GameObject m_fallingLamp;
	[SerializeField, BoxGroup("References")] private Physics3DInteraction m_physics3DInteraction;

	[SerializeField, BoxGroup("Settings")] private float m_lampFallDuration = 1.5f;

	private Coroutine m_flameCoroutine;
	private WaitForSeconds m_delayBetweenFireTick = new WaitForSeconds(1f);

	protected override void Start()
	{
		base.Start();

		m_flameToActivate.SetActive(false);

		m_physics3DInteraction.TriggerEnter3D += StartFireDamage;
		m_physics3DInteraction.TriggerExit3D -= StartFireDamage;

		m_physics3DInteraction.TriggerExit3D += StopFireDamage;
	}

	public void StartFireDamage(Collider _collider)
	{
		m_flameCoroutine = StartCoroutine(FireDamage());
	}

	private void StopFireDamage ( Collider _collider )
	{
		StopCoroutine(m_flameCoroutine);
	}

	// Will stop either by "Player exit" or by "m_scoreLeftToGain is 0"
	private IEnumerator FireDamage()
	{
		// maybe will stop when all score hit
		InflictSplitDamageToPlayer(10);

		FireDamageFxs();

		yield return m_delayBetweenFireTick;

		if (m_scoreLeftToGain > 0)
		{
			m_flameCoroutine = StartCoroutine(FireDamage());
		}
		else
		{
			m_physics3DInteraction.TriggerExit3D -= StopFireDamage;
		}
	}

	private void FireDamageFxs()
	{
		// rename this whatever you want
	}

	[Button]
	public void Cut()
	{
		Vector3 posToFall = new Vector3(m_fallingLamp.transform.position.x, 0, m_fallingLamp.transform.position.z);

		m_flameToActivate.transform.position = new Vector3(posToFall.x, m_flameToActivate.transform.position.y, posToFall.z);

		m_fallingLamp.transform.DOMove(posToFall, m_lampFallDuration).SetEase(Ease.InExpo).OnComplete(
			() =>
			{
				m_fallingLamp.gameObject.SetActive(false);
				m_flameToActivate.SetActive(true);
			});
	}
}
