using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class CombustionTrap : Trap, ICutable
{
	[SerializeField, BoxGroup("References")] private GameObject m_flameToActivate;
	[SerializeField, BoxGroup("References")] private GameObject m_fallingLamp;
	[SerializeField, BoxGroup("References")] private Physics3DInteraction m_physics3DInteraction;

	[SerializeField, BoxGroup("Settings")] private float m_lampFallDuration = 1.5f;
	[SerializeField, BoxGroup("Settings")] private float m_timeBetweenTickDefault = 1f;
	[SerializeField, BoxGroup("Settings"), ReadOnly] private float m_timeBetweenTick = 0;
	[SerializeField, BoxGroup("Settings")] private float m_fireDurationLeftDefault = 6f;
	[SerializeField, BoxGroup("Settings"), ReadOnly] private float m_fireDurationLeft = 6f;
	[SerializeField, BoxGroup("Settings")] private bool m_isPlayerInFire;

	private Vector3 m_fallingLampBasePos;

	protected override void Awake()
	{
		m_fallingLampBasePos = m_fallingLamp.transform.position;
	}

	protected override void Start()
	{
		base.Start();

		m_physics3DInteraction.TriggerStay3D += (x) =>
		{
			if (x.gameObject.layer == 6)
			{
				m_isPlayerInFire = true;
			}
		};

		m_physics3DInteraction.TriggerExit3D += StopFireDamage;
	}

	protected override void Init()
	{
		base.Init();

		m_fallingLamp.transform.position = m_fallingLampBasePos;

		if (!m_fallingLamp.gameObject.activeSelf) m_fallingLamp.gameObject.SetActive(true);
		m_flameToActivate.SetActive(false);

		m_fireDurationLeft = m_fireDurationLeftDefault;

		m_isPlayerInFire = false;
	}

	private void StopFireDamage ( Collider _collider)
	{
		if (_collider.gameObject.layer == 6)
		{
			m_isPlayerInFire = false;
		}
	}

	private void Update()
	{
		if (!m_isTrapActive)
			return;

		if (m_fireDurationLeft <= 0)
		{
			ResetTrap();
			return;
		}
		m_fireDurationLeft -= Time.deltaTime;

		if (!m_isPlayerInFire)
			return;

		if (m_timeBetweenTick > 0)
		{
			m_timeBetweenTick -= Time.deltaTime;
		}
		else
		{
			InflictSplitDamageToPlayer(10);

			FireDamageFxs();

			m_timeBetweenTick = m_timeBetweenTickDefault;
		}
	}

	public override void ResetTrap()
	{
		base.ResetTrap();

		m_isTrapActive = false;
		m_timeBetweenTick = 0;
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

				m_isTrapActive = true;
			});
	}
}
