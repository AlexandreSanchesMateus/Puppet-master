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
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _maxHealth;

        [SerializeField] private Entity _entityRef;

        public event Action<int> OnDamage;

        public event Action OnDie;

        public void TakeDamage(int amount)
        {
            Assert.IsTrue(amount >= 0);

            OnDamage?.Invoke(amount);

            DamageEffect();
        }
        
        private void DamageEffect()
        {
	        
		}

        private void OnDestroy()
        {

        }
    }
}
