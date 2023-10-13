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
	    public event Action<int> OnDamage;
	    public event Action OnDie;

        public Entity Entity => _entityRef;
        [SerializeField] private Entity _entityRef;

        public float score;

		public void TakeDamage(int amount)
        {
            Assert.IsTrue(amount >= 0);

            OnDamage?.Invoke(amount);

            score += amount;

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
