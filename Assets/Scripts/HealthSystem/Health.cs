using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public class Health : MonoBehaviour, IHealth
    {
        [SerializeField] private int _maxHealth;

        [SerializeField] private Entity _entityRef;

		public int CurrentHealth
        {
            get;
            private set;
        }
        public bool IsDead => CurrentHealth <= 0;
        public int MaxHealth { get => _maxHealth; }

        public event Action<int> OnDamage;
        public event Action<int> OnRegen;
        public event Action OnDie;

        public void TakeDamage(int amount)
        {
            Assert.IsTrue(amount >= 0);
            if (IsDead) return;

            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
            OnDamage?.Invoke(amount);

            DamageEffect();

			if (IsDead)
                InternalDie();
        }
        public void Regen(int amount)
        {
            Assert.IsTrue(amount >= 0);
            if (IsDead) return;
            InternalRegen(amount);
        }
        public void Kill()
        {
            if (IsDead) return;
            InternalDie();
        }

        public void Revive()
        {
            Debug.Log("REVIVE !");
            CurrentHealth = _maxHealth;
        }

        void InternalRegen(int amount)
        {
            Assert.IsTrue(amount >= 0);

            var old = CurrentHealth;
            CurrentHealth = Mathf.Min(_maxHealth, CurrentHealth + amount);
            OnRegen?.Invoke(CurrentHealth - old);
        }

        void InternalDie()
        {
            CurrentHealth = 0;
            OnDie?.Invoke();
        }

        private void DamageEffect()
        {
	        
		}

        private void OnDestroy()
        {

        }
    }
}
