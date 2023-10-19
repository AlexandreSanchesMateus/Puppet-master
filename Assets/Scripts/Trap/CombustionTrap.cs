using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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

	[Foldout("Event")] public UnityEvent onCutRope;
	[Foldout("Event")] public UnityEvent onLampTouchGround;
	[Foldout("Event")] public UnityEvent onResetTrap;
	[Foldout("Event")] public UnityEvent onPlayerTriggerStay;
	[Foldout("Event")] public UnityEvent onPlayerTriggerExit;
	[Foldout("Event")] public UnityEvent onPlayerDamage;

	[SerializeField, BoxGroup("Rope")] private Ropes m_ropePrefab;
	[SerializeField, BoxGroup("Rope")] private Ropes m_currentRopes;
	[SerializeField, BoxGroup("Rope")] private Transform m_ropeSpawn;

	private Vector3 m_fallingLampBasePos;

	private void Awake()
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

				onPlayerTriggerStay?.Invoke();
			}
		};

		m_physics3DInteraction.TriggerExit3D += StopFireDamage;

		m_currentRopes.onCut += Cut;
	}

	protected override void Init()
	{
		base.Init();

		m_fallingLamp.transform.position = m_fallingLampBasePos;

		if (!m_fallingLamp.gameObject.activeSelf) m_fallingLamp.gameObject.SetActive(true);
		m_flameToActivate.SetActive(false);

		m_fireDurationLeft = m_fireDurationLeftDefault;

		m_isPlayerInFire = false;

		m_currentRopes.onCut += Cut;
	}

	private void StopFireDamage ( Collider _collider)
	{
		if (_collider.gameObject.layer == 6)
		{
			m_isPlayerInFire = false;

			onPlayerTriggerExit?.Invoke();
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

		Vector3 defaultScale = m_fallingLamp.transform.localScale;

		m_fallingLamp.transform.DOScale(defaultScale, 0.3f).From(0);

		m_currentRopes = Instantiate(m_ropePrefab, m_ropeSpawn.position, Quaternion.identity, m_ropeSpawn);
		m_currentRopes.onCut += Cut;

		onResetTrap?.Invoke();
	}

	private void FireDamageFxs()
	{
		// rename this whatever you want

		onPlayerDamage?.Invoke();
	}

	[Button]
	public void Cut()
	{
		Vector3 posToFall = new Vector3(m_fallingLamp.transform.position.x, 0, m_fallingLamp.transform.position.z);

		m_flameToActivate.transform.position = new Vector3(posToFall.x, m_flameToActivate.transform.position.y, posToFall.z);

		onCutRope?.Invoke();

		m_fallingLamp.transform.DOMove(posToFall, m_lampFallDuration).SetEase(Ease.InExpo).OnComplete(
			() =>
			{
				m_fallingLamp.gameObject.SetActive(false);
				m_flameToActivate.SetActive(true);

				m_isTrapActive = true;

				onLampTouchGround?.Invoke();
			});
	}
}
