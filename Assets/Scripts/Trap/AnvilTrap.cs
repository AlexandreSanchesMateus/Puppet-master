using DG.Tweening;
using Game;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class AnvilTrap : Trap, ICutable
{
	[SerializeField, BoxGroup("References")] private GameObject m_fallingLamp;

	[SerializeField, BoxGroup("Settings")] private float m_lampFallDuration = 1.5f;
	[SerializeField, BoxGroup("Settings")] private float m_timeBetweenTickDefault = 1f;
	[SerializeField, BoxGroup("Settings"), ReadOnly] private float m_timeBetweenTick = 0;
	[SerializeField, BoxGroup("Settings")] private float m_fireDurationLeftDefault = 6f;
	[SerializeField, BoxGroup("Settings"), ReadOnly] private float m_fireDurationLeft = 6f;
	[SerializeField, BoxGroup("Settings")] private bool m_isPlayerInFire;

	[SerializeField] private Rigidbody[] m_allAnvil;

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

	private void Awake ()
	{
		m_fallingLampBasePos = m_fallingLamp.transform.position;
	}

	protected override void Start ()
	{
		base.Start();

		m_currentRopes.onCut += Cut;
	}

	protected override void Init ()
	{
		base.Init();

		m_fallingLamp.transform.position = m_fallingLampBasePos;

		if (!m_fallingLamp.gameObject.activeSelf) m_fallingLamp.gameObject.SetActive(true);

		m_fireDurationLeft = m_fireDurationLeftDefault;

		m_isPlayerInFire = false;

		m_currentRopes.onCut += Cut;
	}

	private void StopFireDamage ( Collider _collider )
	{
		if (_collider.gameObject.layer == 6)
		{
			m_isPlayerInFire = false;

			onPlayerTriggerExit?.Invoke();
		}
	}

	private void Update ()
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

	public override void ResetTrap ()
	{
		base.ResetTrap();

		m_isTrapActive = false;
		m_timeBetweenTick = 0;

		Vector3 defaultScale = m_fallingLamp.transform.localScale;

		m_fallingLamp.transform.DOScale(defaultScale, 0.3f).From(0);

		m_currentRopes = Instantiate(m_ropePrefab, m_ropeSpawn.position, new Quaternion(0, 0, 0, 0), m_ropeSpawn);
		m_currentRopes.onCut += Cut;

		onResetTrap?.Invoke();
	}

	private void FireDamageFxs ()
	{
		// rename this whatever you want

		onPlayerDamage?.Invoke();
	}

	[Button]
	public void Cut ()
	{
		onCutRope?.Invoke();

		foreach (Rigidbody rigidbody in m_allAnvil)
		{
			rigidbody.useGravity = true;
		}
	}
}
